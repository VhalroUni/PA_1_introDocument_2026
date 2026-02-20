using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Sheep", menuName = "Finite State Machines/FSM_Sheep", order = 1)]
public class FSM_Sheep : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    
    private Flee flee;
    private WanderAround wanderAround;
    private SteeringContext context;
    private Blackboard blackboard;
    private GameObject enemy;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<Blackboard>();
        wanderAround = GetComponent<WanderAround>();
        context = GetComponent<SteeringContext>();
        flee = GetComponent<Flee>();
        
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        base.OnExit();
    }

    public override void OnConstruction()
    {
        //STAGE 1: create the states with their logic(s)
        // *-----------------------------------------------
        
        State WanderArraund = new State("OVEJAPASTOREA",
            () => { wanderAround.attractor = blackboard.recinto; wanderAround.enabled = true; context.maxSpeed = blackboard.initialSpeed; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { wanderAround.enabled = false; }  // write on exit logic inisde {}  
        );
        State Huye = new State("OVEJASEMUEVE",
            () => { flee.target = enemy;  flee.enabled = true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { flee.enabled = false; }  // write on exit logic inisde {}  
        );
        State Meta = new State("OVEJASALVADA",
            () => { wanderAround.attractor = blackboard.recinto; context.seekWeight = blackboard.seekMultiplier; blackboard.SheepSaved++; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );
        State OvejaSiendoComida = new State("LAOVEJAESTASIENDOCOMIDA",
            () => { DisableAllSteerings(); }, // write on enter logic inside {}
            () => { blackboard.timerMuerte += Time.deltaTime; }, // write in state logic inside {}
            () => { blackboard.timerMuerte = 0f; }  // write on exit logic inisde {}  
        );
        State Muerte = new State("OVEJAMUERTA",
           () => { gameObject.SetActive(false); blackboard.SheepDeath++; }, // write on enter logic inside {}
           () => { }, // write in state logic inside {}
           () => { }  // write on exit logic inisde {}  
       );

        // STAGE 2: create the transitions with their logic(s)
        //* ---------------------------------------------------

        Transition PerroCerca = new Transition("ElPerroSeHaAcercado",
            () => { return SensingUtils.FindRandomInstanceWithinRadius(gameObject, "REPULSIVE", blackboard.dogInRadius); }, // write the condition checkeing code in {}
            () => { enemy = blackboard.dog; context.maxSpeed *= blackboard.speedMultiplier; }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition PerroLejos = new Transition("ElPerroSeHaAlejado",
            () => { return !(SensingUtils.FindRandomInstanceWithinRadius(gameObject, "REPULSIVE", blackboard.dogInRadius)); }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition LlegagaAMeta = new Transition("LaOvejaSeSalva",
            () => { return true; }, //SensingUtils.FindRandomInstanceWithinRadius(blackboard.recinto, "SHEEP", blackboard.recintoRadius); }, // write the condition checkeing code in {}
            () => { context.maxSpeed = blackboard.speedDebuff; }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition OvejaAlAcecho = new Transition("OvejaSeráAtrapada",
            () => { return SensingUtils.FindRandomInstanceWithinRadius(gameObject, "ATTACKER", blackboard.wolfInRadius); }, // write the condition checkeing code in {}
            () => { enemy = blackboard.wolf; context.maxSpeed *= blackboard.speedMultiplier; }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition OvejaSalvada = new Transition("OvejaSeHaSalvado",
            () => { return true; }, // write the condition checkeing code in {} PENSARLO BIEN
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition OvejaMuriendo = new Transition("OvejaMuriendo",
            () => { return blackboard.timerMuerte < blackboard.deathTime; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        // STAGE 3: add states and transitions to the FSM 
        //* ----------------------------------------------

        AddStates(WanderArraund, Huye, Meta, OvejaSiendoComida, Muerte);

        AddTransition(WanderArraund, PerroCerca, Huye);
        AddTransition(Huye, PerroLejos, WanderArraund);

        AddTransition(Huye, LlegagaAMeta, Meta);

        AddTransition(WanderArraund, OvejaAlAcecho, OvejaSiendoComida);
        AddTransition(OvejaSiendoComida, OvejaSalvada, WanderArraund);

        AddTransition(OvejaSiendoComida, OvejaMuriendo, Muerte);

        // STAGE 4: set the initial state

        initialState = WanderArraund;

         

    }
}

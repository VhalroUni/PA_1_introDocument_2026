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

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        wanderAround = GetComponent<WanderAround>();
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
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );
        State Huye = new State("OVEJASEMUEVE",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );
        State Meta = new State("OVEJASALVADA",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );
        State OvejaSiendoComida = new State("LAOVEJAESTASIENDOCOMIDA",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );
        State Muerte = new State("OVEJAMUERTA",
           () => { }, // write on enter logic inside {}
           () => { }, // write in state logic inside {}
           () => { }  // write on exit logic inisde {}  
       );

        // STAGE 2: create the transitions with their logic(s)
        //* ---------------------------------------------------

        Transition PerroCerca = new Transition("ElPerroSeHaAcercado",
            () => { return true; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition PerroLejos = new Transition("ElPerroSeHaAlejado",
            () => { return true; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition LlegagaAMeta = new Transition("LaOvejaSeSalva",
            () => { return true; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition OvejaAlAcecho = new Transition("OvejaSeráAtrapada",
            () => { return true; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition OvejaSalvada = new Transition("OvejaSeHaSalvado",
            () => { return true; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition OvejaMuriendo = new Transition("OvejaMuriendo",
            () => { return true; }, // write the condition checkeing code in {}
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

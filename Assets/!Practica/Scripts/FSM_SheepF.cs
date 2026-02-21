using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_SheepF", menuName = "Finite State Machines/FSM_SheepF", order = 1)]
public class FSM_SheepF : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private Sheep_BLACKBOARD blackboard;
    private WanderAround wander;
    private SteeringContext context;
    private Flee flee;
    private GameObject enemy;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<Sheep_BLACKBOARD>();
        wander = GetComponent<WanderAround>();
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
        // STAGE 1: create the states with their logic(s)
        // -----------------------------------------------


        State wandering = new State("Wandering arround",
            () => { wander.attractor = blackboard.centerScreen; context.seekWeight *= blackboard.attractorMultiplier; wander.enabled = true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { wander.enabled = false; }  // write on exit logic inisde {}  
        );

        State fleeFromDog = new State("Flee from dog",
            () => { flee.target = enemy; flee.enabled = true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { flee.enabled = false; }  // write on exit logic inisde {}  
        );

        State wolfFear = new State("Fear from wolf",
            () => { blackboard.deathTimer = 0f; }, // write on enter logic inside {}
            () => { blackboard.deathTimer += Time.deltaTime; }, // write in state logic inside {}
            () => { blackboard.deathTimer = 0f; }  // write on exit logic inisde {}  
        );

        State sheepDeath = new State("Sheep is dead",
            () => { gameObject.SetActive(false); }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );




        // STAGE 2: create the transitions with their logic(s)
        // ---------------------------------------------------

        Transition dogChasing = new Transition("Dog is chasing",
            () => { enemy = SensingUtils.FindInstanceWithinRadius(gameObject, "DOG", blackboard.dogInRadius); return enemy != null; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition wolfChasing = new Transition("Wolf is chasing",
            () => { enemy = SensingUtils.FindInstanceWithinRadius(gameObject, "WOLF", blackboard.wolfInRadius); return enemy != null; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition wolfCanEat = new Transition("Wolf is eating",
            () => { return blackboard.deathTimer >= blackboard.deathTime; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition dogNotChasing = new Transition("Dog is not chasing",
            () => { return SensingUtils.DistanceToTarget(gameObject, enemy) > blackboard.dogInRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );



        // STAGE 3: add states and transitions to the FSM 
        // ----------------------------------------------

        AddStates(wandering, fleeFromDog, wolfFear, sheepDeath);

        AddTransition(wandering, dogChasing, fleeFromDog);
        AddTransition(wandering, wolfChasing, wolfFear);
        AddTransition(fleeFromDog, dogNotChasing, wandering);
        AddTransition(wolfFear, dogChasing, fleeFromDog);
        AddTransition(wolfFear, wolfCanEat, sheepDeath);




        // STAGE 4: set the initial state

        initialState = wandering;

    }
}

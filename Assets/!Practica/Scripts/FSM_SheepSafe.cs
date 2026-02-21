using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_SheepSafe", menuName = "Finite State Machines/FSM_SheepSafe", order = 1)]
public class FSM_SheepSafe : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    Sheep_BLACKBOARD blackboard;
    Arrive arrive;
    SteeringContext context;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        blackboard = GetComponent<Sheep_BLACKBOARD>();
        arrive = GetComponent<Arrive>();
        context = GetComponent<SteeringContext>();

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

        FiniteStateMachine sheepBehaviour = ScriptableObject.CreateInstance<FSM_SheepF>();
        sheepBehaviour.Name = "Normal Behaviour";

        State safeZone = new State("Safe zone",
            () => { arrive.target = blackboard.centerSafeZone; arrive.enabled = true;}, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { arrive.enabled = false; }  // write on exit logic inisde {}  
        );

        State safe = new State("Stopped",
            () => { DisableAllSteerings(); }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );


        // STAGE 2: create the transitions with their logic(s)
        // ---------------------------------------------------

        Transition isInSafeZone = new Transition("Is going safe zone",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.safeZone) <= blackboard.safeZoneRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition safeZoneReached = new Transition("Is in safe zone",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.safeZone) <= blackboard.closeEnoughRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );



        // STAGE 3: add states and transitions to the FSM 
        // ----------------------------------------------

        AddStates(sheepBehaviour, safeZone, safe);

        AddTransition(sheepBehaviour, isInSafeZone, safeZone);
        AddTransition(safeZone, safeZoneReached, safe);

        // STAGE 4: set the initial state

        initialState = sheepBehaviour;


    }
}

using FSMs;
using Steerings;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "FSM_Wolf", menuName = "Finite State Machines/FSM_Wolf", order = 1)]
public class FSM_Wolf : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private GameObject sheep;
    private Wolf_BLACKBOARD blackboard;
    private Seek seek;
    private Pursue pursue;
    private WanderAround wander;
    private float pursuingTime;
    private float restingTime;
    private float elapsedTime;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        wander = gameObject.GetComponent<WanderAround>();
        blackboard = gameObject.GetComponent<Wolf_BLACKBOARD>();
        seek = gameObject.GetComponent<Seek>();
        pursue = gameObject.GetComponent<Pursue>();

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
         
        State Hide = new State("HIDE",
            () => { SensingUtils.FindInstanceWithinRadius(gameObject, "SHEEP", 10); }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

        State WanderAround = new State("WANDERAROUND",
            () => { wander.attractor = blackboard.attractor; wander.enabled = true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { wander.enabled = false; }  // write on exit logic inisde {}  
        );

        State Chase = new State("CHASING",
            () => { pursuingTime = 0; pursue.target = sheep; pursue.enabled = true; }, // write on enter logic inside {}
            () => { pursuingTime += Time.deltaTime; }, // write in state logic inside {}
            () => { pursue.enabled = false; }  // write on exit logic inisde {}  
        );

        State Eat = new State("EATING",
            () => { elapsedTime = 0; }, // write on enter logic inside {}
            () => { elapsedTime += Time.deltaTime; }, // write in state logic inside {}
            () => { Destroy(sheep); }  // write on exit logic inisde {}  
        );

        State Rest = new State("RESTING",
            () => { restingTime = 0; }, // write on enter logic inside {}
            () => { restingTime += Time.deltaTime; }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );


        // STAGE 2: create the transitions with their logic(s)
         //* ---------------------------------------------------

        Transition ZoneRadiusReached = new Transition("ZoneRadiusReached",
            () => { return true; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition SheepDetected = new Transition("SheepDetected",
            () => {sheep = SensingUtils.FindInstanceWithinRadius(gameObject, "SHEEP", blackboard.sheepDetectableRaidus);
                return sheep != null;
            }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition SheepReached = new Transition("SheepReached",
            () => {
                return SensingUtils.FindInstanceWithinRadius(gameObject, "SHEEP", blackboard.sheepReachedRadius);
            }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition SheepFlee = new Transition("SheepFlee",
            () => { return true; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition WolfResting = new Transition("WolfResting",
            () => { return true; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        // STAGE 3: add states and transitions to the FSM 
         //* ----------------------------------------------
            
        AddStates(Hide, WanderAround, Chase, Eat, Rest);

        AddTransition(Hide, ZoneRadiusReached, WanderAround);
        AddTransition(WanderAround, SheepDetected, Chase);
        AddTransition(Chase, SheepReached, Eat);
        AddTransition(Chase, SheepFlee, Rest);
        AddTransition(Rest, WolfResting, WanderAround);   


        // STAGE 4: set the initial state

        initialState = Hide;
    }
}

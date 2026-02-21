using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_Wolf", menuName = "Finite State Machines/FSM_Wolf", order = 1)]
public class FSM_Wolf : FiniteStateMachine
{
    private GameObject sheep;
    private Wolf_BLACKBOARD blackboard;
    private Seek seek;
    private Pursue pursue;
    private WanderAround wander;

    private float pursuingTime;
    private float restingTime;
    private float elapsedTime;

    private GameObject bush;

    private bool Ensure()
    {
        if (blackboard == null) blackboard = gameObject.GetComponent<Wolf_BLACKBOARD>();
        if (seek == null) seek = gameObject.GetComponent<Seek>();
        if (pursue == null) pursue = gameObject.GetComponent<Pursue>();
        if (wander == null) wander = gameObject.GetComponent<WanderAround>();
        return blackboard != null && seek != null && pursue != null && wander != null;
    }

    private void DisableAll()
    {
        if (seek != null) seek.enabled = false;
        if (pursue != null) pursue.enabled = false;
        if (wander != null) wander.enabled = false;
    }

    private GameObject FindClosestWithTag(string tag, float radius)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);
        GameObject best = null;
        float bestD = Mathf.Infinity;

        foreach (var c in cols)
        {
            if (!c.CompareTag(tag)) continue;
            float d = Vector3.Distance(transform.position, c.transform.position);
            if (d < bestD)
            {
                bestD = d;
                best = c.gameObject;
            }
        }
        return best;
    }

    public override void OnEnter()
    {
        wander = gameObject.GetComponent<WanderAround>();
        blackboard = gameObject.GetComponent<Wolf_BLACKBOARD>();
        seek = gameObject.GetComponent<Seek>();
        pursue = gameObject.GetComponent<Pursue>();

        base.OnEnter();
    }

    public override void OnExit()
    {
        DisableAll();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        State Hide = new State("HIDE",
            () =>
            {
                Ensure();
                DisableAll();
                bush = FindClosestWithTag(blackboard.bushTag, blackboard.bushSearchRadius);
                if (bush != null)
                {
                    seek.target = bush;
                    seek.enabled = true;
                }
            },
            () => { },
            () =>
            {
                if (seek != null) seek.enabled = false;
            }
        );

        State WanderAround = new State("WANDERAROUND",
            () =>
            {
                Ensure();
                DisableAll();
                wander.attractor = blackboard.attractor;
                wander.enabled = true;
            },
            () => { },
            () =>
            {
                if (wander != null) wander.enabled = false;
            }
        );

        State Chase = new State("CHASING",
            () =>
            {
                Ensure();
                DisableAll();
                pursuingTime = 0f;
                if (sheep != null)
                {
                    pursue.target = sheep;
                    pursue.enabled = true;
                }
            },
            () =>
            {
                Ensure();
                pursuingTime += Time.deltaTime;

                if (sheep == null)
                {
                    sheep = SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.sheepTag, blackboard.sheepDetectableRaidus);
                    if (sheep != null)
                    {
                        pursue.target = sheep;
                        pursue.enabled = true;
                    }
                }
                else if (pursue != null && pursue.enabled)
                {
                    pursue.target = sheep;
                }
            },
            () =>
            {
                if (pursue != null) pursue.enabled = false;
            }
        );

        State Eat = new State("EATING",
            () =>
            {
                Ensure();
                DisableAll();
                elapsedTime = 0f;
                if (sheep != null)
                {
                    seek.target = sheep;
                    seek.enabled = true;
                }
            },
            () =>
            {
                elapsedTime += Time.deltaTime;
                if (sheep != null && seek != null && seek.enabled) seek.target = sheep;
            },
            () =>
            {
                if (seek != null) seek.enabled = false;
                if (sheep != null)
                {
                    Object.Destroy(sheep);
                    sheep = null;
                }
            }
        );

        State Rest = new State("RESTING",
            () =>
            {
                Ensure();
                DisableAll();
                restingTime = 0f;
            },
            () =>
            {
                restingTime += Time.deltaTime;
            },
            () => { }
        );

        Transition ZoneRadiusReached = new Transition("ZoneRadiusReached",
            () =>
            {
                if (!Ensure()) return false;
                if (bush == null) return true;
                return Vector3.Distance(transform.position, bush.transform.position) <= blackboard.hideReachedRadius;
            },
            () => { }
        );

        Transition SheepDetected = new Transition("SheepDetected",
            () =>
            {
                if (!Ensure()) return false;
                sheep = SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.sheepTag, blackboard.sheepDetectableRaidus);
                return sheep != null;
            },
            () => { }
        );

        Transition SheepReached = new Transition("SheepReached",
            () =>
            {
                if (!Ensure()) return false;
                if (sheep == null) return false;
                return Vector3.Distance(transform.position, sheep.transform.position) <= blackboard.sheepReachedRadius;
            },
            () => { }
        );

        Transition SheepFlee = new Transition("SheepFlee",
            () =>
            {
                if (!Ensure()) return false;
                if (sheep == null) return true;
                return pursuingTime >= blackboard.maxPersuingTime;
            },
            () =>
            {
                sheep = null;
            }
        );

        Transition WolfResting = new Transition("WolfResting",
            () =>
            {
                if (!Ensure()) return false;
                return restingTime >= blackboard.restingDuration;
            },
            () => { }
        );

        Transition DoneEating = new Transition("DoneEating",
            () =>
            {
                if (!Ensure()) return false;
                return elapsedTime >= blackboard.eatDuration;
            },
            () => { }
        );

        AddStates(Hide, WanderAround, Chase, Eat, Rest);

        AddTransition(Hide, ZoneRadiusReached, WanderAround);
        AddTransition(WanderAround, SheepDetected, Chase);
        AddTransition(Chase, SheepReached, Eat);
        AddTransition(Chase, SheepFlee, Rest);
        AddTransition(Eat, DoneEating, Rest);
        AddTransition(Rest, WolfResting, WanderAround);

        initialState = Hide;
    }
}
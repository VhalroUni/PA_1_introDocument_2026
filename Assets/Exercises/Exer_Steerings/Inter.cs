using UnityEngine;

namespace Steerings
{

    public class Inter : SteeringBehaviour
    {

        public GameObject TargetA;
        public GameObject TargetB;

        /*
        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)
        public GameObject target;
        */
        public override GameObject GetTarget()
        {
            return TargetA;
        }
        

        public override Vector3 GetLinearAcceleration()
        {
            return Inter.GetLinearAcceleration(Context, TargetA, TargetB);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject TargetA, GameObject TargetB)
        {
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
            Vector3 mid = (TargetA.transform.position + TargetB.transform.position) / 2;

            SURROGATE_TARGET.transform.position = mid; 

            return Arrive.GetLinearAcceleration(me,SURROGATE_TARGET);
        }

    }
}
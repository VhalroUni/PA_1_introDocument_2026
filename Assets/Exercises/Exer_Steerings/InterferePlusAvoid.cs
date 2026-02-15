using UnityEngine;

namespace Steerings
{

    public class InterferePlusAvoid : SteeringBehaviour
    {
        public GameObject target;
        public float requiredDistance;
        /*
        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)
        public GameObject target;
        */
        public override GameObject GetTarget()
        {
            return target;
        }
        

        public override Vector3 GetLinearAcceleration()
        {
            return InterferePlusAvoid.GetLinearAcceleration(Context, target, requiredDistance);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance)
        {
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
            Vector3 oaAcc = ObstacleAvoidance.GetLinearAcceleration(me);
            if (oaAcc!= Vector3.zero) return oaAcc;
            else
                return Interfere.GetLinearAcceleration(me, requiredDistance, target);
        }

    }
}
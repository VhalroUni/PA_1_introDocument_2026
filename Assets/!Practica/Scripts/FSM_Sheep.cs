using UnityEngine;

namespace Steerings
{

    public class FSM_Sheep : SteeringBehaviour
    {

        /*
        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)
        public GameObject target;

        public override GameObject GetTarget()
        {
            return target;
        }
        */
        
        public override Vector3 GetLinearAcceleration()
        {
            return FSM_Sheep.GetLinearAcceleration(Context /* add extra parameters (target?) if required */);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me /* add extra parameters (target?) if required */)
        {
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
        }

    }
}
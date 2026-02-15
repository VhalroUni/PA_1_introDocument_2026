using UnityEngine;
using UnityEngine.UIElements;

namespace Steerings
{

    public class KeepDistance : SteeringBehaviour
    {
   
        public GameObject target;
        public float requiredDistance;

        public override GameObject GetTarget()
        {
            return target;
        }
     
        
        public override Vector3 GetLinearAcceleration()
        {
            /* COMPLETE */
            //return Vector3.zero; // remove this line when exercise completed
            return KeepDistance.GetLinearAcceleration(Context, target, requiredDistance);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance)
        {

            /* COMPLETE */
            Vector3 directionFromTarget = me.transform.position - target.transform.position;
            Vector3 displacementFromTarget = directionFromTarget.normalized * requiredDistance;
            Vector3 desiredPosition = target.transform.position + displacementFromTarget;

            // return Seek.GetLinearAcceleration(me, SURROGATE_TARGET);
            SURROGATE_TARGET.transform.position = desiredPosition;

            // In the agent's SteeringContext, parameters for arrive should be set to  1, 20, 0.1f
            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        
        }

    }
}
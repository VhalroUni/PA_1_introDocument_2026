using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace Steerings
{

    public class Interfere : SteeringBehaviour
    {
        public float requiredDistance;

        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)
        public GameObject target;

        public override GameObject GetTarget()
        {
            return target;
        }


        public override Vector3 GetLinearAcceleration()
        {
            return Interfere.GetLinearAcceleration(Context, requiredDistance, target);
        }


        public static Vector3 GetLinearAcceleration(SteeringContext me, float requiredDistance, GameObject target)
        {
            SteeringContext targetContext = target.GetComponent<SteeringContext>();
            Vector3 velocityNormalized = targetContext.velocity.normalized;
            Vector3 requiredPosition = target.transform.position + (velocityNormalized * requiredDistance);
            SURROGATE_TARGET.transform.position = requiredPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
        }

    }
}
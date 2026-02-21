using UnityEngine;

namespace Steerings
{

    public class Dog : SteeringBehaviour
    {
        public float movSpeed = 5f;
        public float rotationSpeed = 100f;
        public float sprintMultiplier = 1.5f;

        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            bool sprint = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = movSpeed;

            if(sprint)
                currentSpeed *= sprintMultiplier;

            rb.linearVelocity = transform.up * movSpeed;

            float rotation = 0f;

            if (Input.GetKey(KeyCode.A))
                rotation = rotationSpeed;

            if(Input.GetKey(KeyCode.D))
                rotation = -rotationSpeed;

            rb.MoveRotation(rb.rotation + rotation * Time.deltaTime);
        }
        /*
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
            return Dog.GetLinearAcceleration(Context /* add extra parameters (target?) if required );
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me /* add extra parameters (target?) if required )
        {
            /* COMPLETE this method. It must return the linear acceleration (Vector3) *
        }
        */
    }
}
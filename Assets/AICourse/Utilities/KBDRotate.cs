using UnityEngine;


public class KBDRotate : MonoBehaviour {

	public float angularSpeed = 90;
	public float spirntMultiplier = 1.5f;

	// Update is called once per frame
	void Update () {


		// RIGHT ARROW (KEY) => rotate clockwise (add)
		// LEFT ARROW (KEY) => rotate anticlockwise (substract)


		float move = Input.GetAxis("Horizontal");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

		float currentSpeed = angularSpeed;
		if(isSprinting)
			currentSpeed *= spirntMultiplier;

        float orientation = transform.eulerAngles.z;
		orientation = orientation - move * currentSpeed * Time.deltaTime;
		transform.rotation = Quaternion.Euler(0, 0, orientation);
	}
}
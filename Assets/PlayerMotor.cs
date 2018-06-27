using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

	private CharacterController controller;
	private Vector3 moveVector;

	private float Speed = 15.0f;
	private float verticalVelocity = 0.0f;
	private float gravity = 12.0f;

	//private float animationDuration = 0.0f;

	private int coinScore = 0;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	}

	// Update is called once per frame
	void Update () {

		//if (Time.time < animationDuration){
		//	controller.Move (Vector3.forward * Speed * Time.deltaTime);
		//	return;
		//}

		moveVector = Vector3.zero;

		if (controller.isGrounded) {
			verticalVelocity = -0.5f;
		}

		else
		{
			verticalVelocity -= gravity * Time.deltaTime;	
		}


		// X - L and R
		moveVector.x = Input.GetAxisRaw("Horizontal") * Speed ;

		// Y - Up and Down
		moveVector.y = verticalVelocity;

		// Z - Forward and Backward
		moveVector.z = Speed;

		controller.Move (moveVector * Time.deltaTime);


	}


	//Get Coin
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.CompareTag ("Coin")) {
			Destroy (hit.gameObject);
			coinScore++;
			Debug.Log ("Coin Score = " + coinScore);
		}

	}

}

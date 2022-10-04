using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Basic_controller : MonoBehaviour {

	public InputActionReference inputAction;
	private CharacterController controller;
	public float speed = 6.0f;
	public float turnSpeed = 5.0f;
	public float gravity = 20.0f;
	public float runSpeed = 1.7f;
	private Vector3 moveDirection = Vector3.zero;
	
	
	// Use this for initialization
	void Start () 
	{
		inputAction.action.Enable();
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{	
		
	/*	if (Input.GetKey ("up")) {		 //moving

		 	   runSpeed = 2.6f;
			}							

*/
		
		
		if(controller.isGrounded)
		{
			moveDirection=transform.forward * inputAction.action.ReadValue<Vector2>().y * speed * runSpeed;
			
		}
		float turn = inputAction.action.ReadValue<Vector2>().x;
		transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
		controller.Move(moveDirection * Time.deltaTime);
		moveDirection.y -= gravity * Time.deltaTime;
		
		
		
	}
}
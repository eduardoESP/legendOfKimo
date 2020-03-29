/**
 * @file   PlayerMovement.cs
 * 
 * @authors  Eduardo S Pino, Brackeys
 * 
 * @version 1.0
 * @date 29/03/2020 (DD/MM/YYYY)
 *
 * This component is a repurposed PlayerMovement from Brackeys with minor changes by me.
 * This component checks for players movement inputs and implements helper functions to 
 * freeze/ unfreeze the player and endable/disable animations.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;


	// Update is called once per frame
	void Update()
	{

		if (GameController.controler.state != GameController.STATES.RUNNING)
			return;

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		}
		else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}

	}

	public void OnLanding(bool isJumping)
	{
		animator.SetBool("IsJumping", isJumping);
	}


	public void OnCrouching(bool isCrouching)
	{
		animator.SetBool("IsCrouching", isCrouching);
	}

	void FixedUpdate()
	{
		if (GameController.controler.state == GameController.STATES.PAUSE)
			return;

		controller.Move(crouch, jump);
		jump = false;
	}

	public void FreezePlayer()
	{
		controller.GetRigidBody2D().constraints = RigidbodyConstraints2D.FreezeAll;
	}

	public void UnFreezePlayer()
	{
		controller.GetRigidBody2D().constraints = RigidbodyConstraints2D.None;
		controller.GetRigidBody2D().constraints = RigidbodyConstraints2D.FreezeRotation;
	}
}
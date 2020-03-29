/**
 * @file   CharacterController2D.cs
 * 
 * @authors  Eduardo S Pino, Brackeys
 * 
 * @version 1.0
 * @date 29/03/2020 (DD/MM/YYYY)
 *
 * This component is a repurposed CharacterController2D from Brackeys with minor changes by me.
 * It implements the character move function and checks if character has a ceiling over his head
 * or ground below his feet.
 * 
 */

using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings


	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching



	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;

	private bool hasCeiling = false;
	


	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;

	public BoolEvent OnLandEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new BoolEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void Update()
	{

		if (GameController.controler.state != GameController.STATES.RUNNING)
		{
			return;
		}

		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
		{
			hasCeiling = true;
			OnLandEvent.Invoke(false);
		}
		else
			hasCeiling = false;


		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke(false);
			}
		}


	}

	public void Move(bool crouch, bool jump)
	{
		
		if (!crouch && m_wasCrouching)
		{
			if (hasCeiling)
				crouch = true;
		}

		if (m_Grounded)
		{

			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}


		}
		if (m_Grounded && jump && !hasCeiling)
		{
			m_Grounded = false;
			OnLandEvent.Invoke(true);
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	public Rigidbody2D GetRigidBody2D()
	{
		return  m_Rigidbody2D;
	}

}

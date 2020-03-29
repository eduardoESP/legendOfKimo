/**
 * @file   MoveLeft.cs
 * 
 * @authors  Eduardo S Pino, Alexander Zotov
 * 
 * @version 1.0
 * @date 29/03/2020 (DD/MM/YYYY)
 *
 * This component is a repurposed MoveLeftCycle.cs from Zotov's T-Rex Run Game for Android.
 * It just moves the scenery left.
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{

	[SerializeField]
	float moveSpeed = 5f;
	[SerializeField]
	float leftWayPointX = -8f, rightWayPointX = 8f;

	// Update is called once per frame
	void Update()
	{
		if (GameController.controler.state == GameController.STATES.PAUSE)
		{
			return;
		}
		transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime,
			transform.position.y);

		if (transform.position.x < leftWayPointX)
			transform.position = new Vector2(rightWayPointX, transform.position.y);
	}
}

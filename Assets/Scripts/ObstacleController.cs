/**
 * @file   ObstacleController.cs
 * 
 * @authors  Eduardo S Pino, Alexander Zotov
 * 
 * @version 1.0
 * @date 29/03/2020 (DD/MM/YYYY)
 *
 * This component is a repurposed ObstacleController.cs from Zotov's T-Rex Run Game for Android.
 * It just moves the obstacles to the left and destroys them after they move past the camera
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
	[SerializeField]
	float moveSpeed = -5f;

	[SerializeField]
	Transform spawnPoint;

	[SerializeField]
	bool fixedSpawnPosition = false;

	private void Start()
	{
		if (!fixedSpawnPosition)
		{
			int randomY = Random.Range(0, 20);
			
			spawnPoint.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + randomY*(float)0.1);
			transform.position = spawnPoint.position;
		}
			transform.Rotate(0, 0, 90);
	}
	// Update is called once per frame
	void Update()
	{
		if (GameController.controler.state == GameController.STATES.PAUSE)
		{
			return;
		}

		if (GameController.controler.state == GameController.STATES.RESET)
		{
			Destroy(gameObject);
		}

		transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime,
			transform.position.y);
		

		if (transform.position.x < -13f)
			Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.name.Equals("Player"))
			GameController.controler.PlayerHit();

	}

}

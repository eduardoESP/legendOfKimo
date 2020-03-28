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
		Debug.Log("Hit");
	}

}

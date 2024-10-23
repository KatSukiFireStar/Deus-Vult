using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	[SerializeField] 
	private string sceneName;
	
	[SerializeField] 
	private Vector2 playerPosition;
	
	[SerializeField] 
	private float minXBoundary;
	
	[SerializeField] 
	private float maxXBoundary;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Je change de scene pour scene: " + sceneName);
			SceneManager.LoadScene(sceneName);
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			player.transform.position = playerPosition;
			player.transform.GetChild(0).position = new(0 + playerPosition.x, 3.5f + playerPosition.y, -10);
			CameraMaxBoundary bound = player.transform.GetChild(0).GetComponent<CameraMaxBoundary>();
			bound.MaxX = maxXBoundary;
			bound.MinX = minXBoundary;
		}
		
	}
}
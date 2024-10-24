using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
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
	
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Je change de scene pour scene: " + sceneName);
			animator.SetTrigger("FadeOut");
		}
		
	}

	public void OnFadeComplete()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
				
		SceneManager.LoadScene(sceneName);
		player.transform.position = playerPosition;
		player.transform.GetChild(0).position = new(0 + playerPosition.x, 3.5f + playerPosition.y, -10);
		CameraMaxBoundary bound = player.transform.GetChild(0).GetComponent<CameraMaxBoundary>();
		bound.MaxX = maxXBoundary;
		bound.MinX = minXBoundary;
	}
}
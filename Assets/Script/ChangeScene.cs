using System;
using System.ComponentModel;
using EventSystem.SO;
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
	
	[SerializeField] 
	private BoolEventSO fadeEvent;
	
	private Animator animator;
	private bool here;

	private void Awake()
	{
		fadeEvent.PropertyChanged += FadeEventOnPropertyChanged;
		here = false;
	}

	private void FadeEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		if (s.Value && here)
		{
			OnFadeComplete();
		}
	}

	private void Start()
	{
		animator = GameObject.FindGameObjectWithTag("Player").transform.GetComponentsInChildren<Animator>()[1];
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("PlayerCollider"))
		{
			here = true;
			Debug.Log("Je change de scene pour scene: " + sceneName);
			animator.SetTrigger("FadeOut");
		}
		
	}

	public void OnFadeComplete()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
				
		SceneManager.LoadScene(sceneName);
		player.transform.position = playerPosition;
		player.transform.GetChild(0).position = new(0 + playerPosition.x, 3f + playerPosition.y, -10);
		CameraMaxBoundary bound = player.transform.GetChild(0).GetComponent<CameraMaxBoundary>();
		bound.MaxX = maxXBoundary;
		bound.MinX = minXBoundary;
		animator.SetTrigger("FadeOut");
		here = false;
	}
}
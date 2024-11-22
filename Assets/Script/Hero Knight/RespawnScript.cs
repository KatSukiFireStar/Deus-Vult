using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnScript : MonoBehaviour
{
	[SerializeField] 
	private RespawnEventSO respawnEvent;
	
	[SerializeField] 
	private BoolEventSO respawnBoolEvent;
	
	[SerializeField] 
	private BoolEventSO fadeEvent;

	private Animator animator;
	private bool here;
	
	private void Awake()
	{
		animator = gameObject.transform.GetComponentsInChildren<Animator>()[1];
		here = false;
		respawnBoolEvent.PropertyChanged += RespawnBoolEventOnPropertyChanged;
		fadeEvent.PropertyChanged += FadeEventOnPropertyChanged;
	}

	private void FadeEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		if (s.Value && here)
		{
			SceneManager.LoadScene(respawnEvent.Value.sceneName);
			gameObject.transform.position = respawnEvent.Value.respawnPosition;
			gameObject.transform.GetChild(0).position = new(0 + respawnEvent.Value.respawnPosition.x, 3f + respawnEvent.Value.respawnPosition.y, -10);
			CameraMaxBoundary bound = gameObject.transform.GetChild(0).GetComponent<CameraMaxBoundary>();
			bound.MaxX = respawnEvent.Value.maxXBoundary;
			bound.MinX = respawnEvent.Value.minXBoundary;
			animator.SetTrigger("FadeOut");
			here = false;
		}
		
	}

	private void RespawnBoolEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		//ToDo: Faire le respawn et faire le fade
		
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		if (s.Value)
		{
			animator.SetTrigger("FadeOut");
			here = true;
		}
	}
	
	
}
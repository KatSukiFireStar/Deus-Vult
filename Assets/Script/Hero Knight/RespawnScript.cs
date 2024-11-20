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

	private void Awake()
	{
		respawnBoolEvent.PropertyChanged += RespawnBoolEventOnPropertyChanged;
	}

	private void RespawnBoolEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		//ToDo: Faire le respawn et faire le fade
		
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		if (s.Value)
		{
			SceneManager.LoadScene(respawnEvent.Value.sceneName);
			gameObject.transform.position = respawnEvent.Value.respawnPosition;
			gameObject.transform.GetChild(0).position = new(0 + respawnEvent.Value.respawnPosition.x, 3f + respawnEvent.Value.respawnPosition.y, -10);
			CameraMaxBoundary bound = gameObject.transform.GetChild(0).GetComponent<CameraMaxBoundary>();
			bound.MaxX = respawnEvent.Value.maxXBoundary;
			bound.MinX = respawnEvent.Value.minXBoundary;
		}
	}
}
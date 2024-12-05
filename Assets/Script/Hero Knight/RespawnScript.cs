using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Takes care of respawning the player when they die.
 * Knows the current scene and respawn point
 */
public class RespawnScript : MonoBehaviour
{
	[SerializeField] 
	private RespawnEventSO respawnEvent;

	[SerializeField] 
	private RespawnEventSO changeSceneEvent;
	
	[SerializeField] 
	private BoolEventSO respawnBoolEvent;

	private Animator animator;
	
	private void Awake()
	{
		animator = gameObject.transform.GetComponentsInChildren<Animator>()[1];
		respawnBoolEvent.PropertyChanged += RespawnBoolEventOnPropertyChanged;
	}

	private void OnDestroy()
	{
		respawnBoolEvent.PropertyChanged -= RespawnBoolEventOnPropertyChanged;
	}


	private void RespawnBoolEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		//Respawn the player and do a fade on
		
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		if (s.Value)
		{
			changeSceneEvent.Value = respawnEvent.Value;
			animator.SetTrigger("FadeOut");
		}
	}
	
	
}
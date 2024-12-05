using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] 
	private BoolEventSO pauseEvent;

	private void Awake()
	{
		pauseEvent.PropertyChanged += PauseEventOnPropertyChanged;
		gameObject.SetActive(false);
	}

	private void PauseEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		//Called if the value of the event is changed and apply the value to the object visibility
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		gameObject.SetActive(s.Value);
	}

	public void Continue()
	{
		//Stop the pause in the menu
		pauseEvent.Value = false;
	}
	
	public void Quit()
	{
		//Quit the application
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
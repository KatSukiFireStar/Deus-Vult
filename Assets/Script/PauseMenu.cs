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
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		gameObject.SetActive(s.Value);
	}

	public void Continue()
	{
		pauseEvent.Value = false;
	}
	
	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
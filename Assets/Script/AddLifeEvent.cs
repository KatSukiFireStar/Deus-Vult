using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

public class AddLifeEvent : MonoBehaviour
{
	private LifeBar lifeBar;
	
	[SerializeField] 
	private int life;

	private IntEventSO eventSO;
	
	private void Awake()
	{
		lifeBar = GetComponent<LifeBar>();
		lifeBar.LifePoint = life;
		eventSO = ScriptableObject.CreateInstance<IntEventSO>();
		eventSO.name = "LifeBar " + transform.parent.parent.name;
		eventSO.PropertyChanged += EventSOOnPropertyChanged;
		eventSO.Value = life;
		lifeBar.Life = eventSO;
		lifeBar.AddSuscribe();

		LifeManagerEnnemi lme = transform.parent.parent.GetComponent<LifeManagerEnnemi>();
		lme.LifeEvent = eventSO;
		lme.AddSuscribeLife();
	}

	private void OnDestroy()
	{
		eventSO.PropertyChanged -= EventSOOnPropertyChanged;
	}

	private void Start()
	{
		transform.parent.gameObject.SetActive(false);
	}

	private void EventSOOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		if (s.Value < life)
		{
			transform.parent.gameObject.SetActive(true);
		}
	}
}
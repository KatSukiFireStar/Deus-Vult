using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

public class LifeManagerDemonSlime : MonoBehaviour
{
	[SerializeField]
	private int life = 50;
	
	[SerializeField] 
	private int life2ndPhase = 250;
	
	[SerializeField] 
	private IntEventSO lifeEvent;
	
	public IntEventSO LifeEvent
	{
		get => lifeEvent;
		set => lifeEvent = value;
	}
	
	[SerializeField] 
	private BoolEventSO takeDamageEventSO;
	
	public BoolEventSO TakeDamageEventSO
	{
		get => takeDamageEventSO;
		set => takeDamageEventSO = value;
	}

	[SerializeField]
	private BoolEventSO deadEventSO;
	
	[SerializeField]
	private BoolEventSO transformEventSO;

	private void Awake()
	{
		lifeEvent.Value = life;
		transformEventSO.Value = false;
		transformEventSO.PropertyChanged += TransformEventSOOnPropertyChanged;
		lifeEvent.PropertyChanged += LifeEventOnPropertyChanged;
	}

	private void OnDestroy()
	{
		transformEventSO.PropertyChanged -= TransformEventSOOnPropertyChanged;
		lifeEvent.PropertyChanged -= LifeEventOnPropertyChanged;
		transformEventSO.Value = false;
	}

	private void TransformEventSOOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		transform.GetChild(0).gameObject.SetActive(false);
	}

	public void ReSuscribeLife()
	{
		lifeEvent.Value = life2ndPhase;
		lifeEvent.PropertyChanged += LifeEventOnPropertyChanged;
	}

	private void LifeEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		takeDamageEventSO.Value = true;
		if (s.Value <= 0 && transformEventSO.Value)
		{
			deadEventSO.Value = true; 
			lifeEvent.PropertyChanged -= LifeEventOnPropertyChanged;
			s.Value = 0;
			lifeEvent.PropertyChanged += LifeEventOnPropertyChanged;
		}else if (s.Value <= 0 && !transformEventSO.Value)
		{
			transformEventSO.Value = true;
		}
	}
}
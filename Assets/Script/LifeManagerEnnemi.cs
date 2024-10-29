using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

public class LifeManagerEnnemi : MonoBehaviour
{
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

	private BoolEventSO deadEventSO;

	private void Awake()
	{
		BoolEventSO deadEvent = ScriptableObject.CreateInstance<BoolEventSO>();
		BanditBehaviour bb = gameObject.GetComponent<BanditBehaviour>();
		bb.DeadEvent = deadEvent;
		bb.AddSuscribeDead();
		deadEventSO = deadEvent;
	}

	public void AddSuscribeLife()
	{
		lifeEvent.PropertyChanged += LifeEventOnPropertyChanged;
	}

	private void LifeEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		takeDamageEventSO.Value = true;
		if (s.Value <= 0)
		{
			deadEventSO.Value = true; 
		}
	}
}
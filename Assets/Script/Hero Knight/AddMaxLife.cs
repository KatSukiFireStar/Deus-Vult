using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

public class AddMaxLife : MonoBehaviour
{
	[SerializeField] 
	private IntEventSO amountToAddEvent;
	
	[SerializeField] 
	private IntEventSO maxLifeEvent;

	private void Awake()
	{
		amountToAddEvent.PropertyChanged += AmountToAddEventOnPropertyChanged;
	}

	private void OnDestroy()
	{
		amountToAddEvent.PropertyChanged -= AmountToAddEventOnPropertyChanged;
	}

	private void AmountToAddEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		
		maxLifeEvent.Value += s.Value;
	}
}
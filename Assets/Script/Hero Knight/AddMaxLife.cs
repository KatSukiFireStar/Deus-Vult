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
		//Use to add an amount of life to the max life event of the player 
		//HeartReceptacle
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		
		maxLifeEvent.Value += s.Value;
	}
}
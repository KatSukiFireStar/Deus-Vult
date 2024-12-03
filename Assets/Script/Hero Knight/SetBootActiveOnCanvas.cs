using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

public class SetBootActiveOnCanvas : MonoBehaviour
{
	[SerializeField] 
	private BoolEventSO bootPickupEvent;


	private void Awake()
	{
		bootPickupEvent.PropertyChanged += BootPickupEventOnPropertyChanged;
	}

	private void BootPickupEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		if (s.Value)
		{
			GetComponent<SpriteRenderer>().enabled = true;
		}
	}
}
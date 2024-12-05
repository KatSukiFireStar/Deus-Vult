using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

/*
 * Remove the door when you kill a boss
 * versatile enough for various objects and mobs
 */
public class RemoveObjectOnDeathEvent : MonoBehaviour
{
	[SerializeField] 
	private BoolEventSO deathEvent;

	private void Awake()
	{
		deathEvent.PropertyChanged += DeathEventOnPropertyChanged;
	}

	private void DeathEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		if (s.Value)
		{
			deathEvent.PropertyChanged -= DeathEventOnPropertyChanged;
			Destroy(gameObject);
		}
	}
}
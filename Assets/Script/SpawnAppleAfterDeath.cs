using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnAppleAfterDeath : MonoBehaviour
{
	[SerializeField] 
	private GameObject applePrefab;
	
	[SerializeField] 
	private BoolEventSO deathEvent;

	public BoolEventSO DeathEvent
	{
		get => deathEvent;
		set => deathEvent = value;
	}

	public void SuscribeToDeathEvent()
	{
		deathEvent.PropertyChanged += DeathEventOnPropertyChanged;
	}

	private void DeathEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		if (s.Value)
		{
			if (Random.Range(0, 2) == 0)
			{
				Instantiate(applePrefab, new(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
			}
		}
	}
}
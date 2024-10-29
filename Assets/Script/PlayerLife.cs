using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
	[Header("Events")]
	[SerializeField] 
	private IntEventSO playerLife;
	
	[SerializeField] 
	private IntEventSO damageEvent;
	
	[SerializeField] 
	private BoolEventSO blockingEvent;
	
	[SerializeField] 
	private BoolEventSO rollingEvent;
	
	[SerializeField] 
	private BoolEventSO hurtingEvent;


	private bool blocking = false;
	private bool rolling = false;

	private void Awake()
	{
		damageEvent.PropertyChanged += DamageEventOnPropertyChanged;
		blockingEvent.PropertyChanged += BlockingEventOnPropertyChanged;
		rollingEvent.PropertyChanged += RollingEventOnPropertyChanged;
		playerLife.PropertyChanged += PlayerLifeOnPropertyChanged;
	}

	private void Start()
	{
		playerLife.Value = 200;
	}

	private void PlayerLifeOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		Debug.Log(s.Value);
	}

	private void RollingEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		rolling = s.Value;
	}

	private void BlockingEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		blocking = s.Value;
	}

	private void DamageEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		if (!rolling && !blocking)
		{
			playerLife.Value -= s.Value;
			hurtingEvent.Value = true;
		}else if (blocking && !rolling)
		{
			playerLife.Value -= (s.Value / 2);
		}
	}
}
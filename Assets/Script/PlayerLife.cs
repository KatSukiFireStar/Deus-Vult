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
	private IntEventSO _healingEvent;
	
	[SerializeField] 
	private BoolEventSO blockingEvent;
	
	[SerializeField] 
	private BoolEventSO hurtingEvent;

	[SerializeField] 
	private IntEventSO _maxLifeEvent;
	

	private bool blocking = false;
	private bool rolling = false;

	private void Awake()
	{
		damageEvent.PropertyChanged += DamageEventOnPropertyChanged;
		blockingEvent.PropertyChanged += BlockingEventOnPropertyChanged;
		playerLife.PropertyChanged += PlayerLifeOnPropertyChanged;
		_healingEvent.PropertyChanged += HealingEventOnPropertyChanged;
	}

	private void HealingEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		playerLife.Value += s.Value;

	}

	private void Start()
	{
		playerLife.Value = 200;
		_maxLifeEvent.Value = 200;
	}

	private void PlayerLifeOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		//Todo : Ajouter gestion de la vie quand elle atteint zero
		if (playerLife.Value > _maxLifeEvent.Value)
		{
			playerLife.Value = _maxLifeEvent.Value;
		}
	}

	private void BlockingEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
		blocking = s.Value;
	}

	private void DamageEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		//Todo : Verifiez que la vie ne descend pas en dessous de 0
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
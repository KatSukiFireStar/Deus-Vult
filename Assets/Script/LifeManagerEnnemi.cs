using System;
using System.ComponentModel;
using EventSystem.SO;
using Script;
using UnityEngine;

public class LifeManagerEnnemi : MonoBehaviour
{
	[SerializeField] 
	private IntEventSO lifeEvent;

	[SerializeField] 
	private EnnemiEnum ennemiType;

	private BanditBehaviour _banditBehaviour;
	
	private SkelettonBehaviour _skelettonBehaviour;
	
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
		
		if (ennemiType == EnnemiEnum.Bandit)
		{
			_banditBehaviour = gameObject.GetComponent<BanditBehaviour>();
			_banditBehaviour.DeadEvent = deadEvent;
			_banditBehaviour.AddSuscribeDead();
		} else if (ennemiType == EnnemiEnum.Skeleton)
		{
			_skelettonBehaviour = gameObject.GetComponent<SkelettonBehaviour>();
			_skelettonBehaviour.DeadEvent = deadEvent;
			_skelettonBehaviour.AddSuscribeDead();
		}
		deadEventSO = deadEvent;
	}

	public void AddSuscribeLife()
	{
		lifeEvent.PropertyChanged += LifeEventOnPropertyChanged;
	}

	private void OnDestroy()
	{
		lifeEvent.PropertyChanged -= LifeEventOnPropertyChanged;
	}

	private void LifeEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		if (takeDamageEventSO != null)
			takeDamageEventSO.Value = true;
		if (s.Value <= 0)
		{
			deadEventSO.Value = true; 
		}
	}
}
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
		transformEventSO.PropertyChanged += TransformEventSOOnPropertyChanged;
	}

	private void Start()
	{
		lifeEvent.Value = life;
	}

	private void TransformEventSOOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		lifeEvent.Value = life2ndPhase;
		Destroy(transform.GetChild(0));
	}

	private void LifeEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;
		takeDamageEventSO.Value = true;
		if (s.Value <= 0 && transformEventSO.Value)
		{
			deadEventSO.Value = true; 
		}else if (s.Value <= 0 && !transformEventSO.Value)
		{
			transformEventSO.Value = true;
		}
	}
}
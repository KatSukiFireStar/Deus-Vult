using System;
using EventSystem.SO;
using Script;
using UnityEngine;

public class AddTakeDamageEvent : MonoBehaviour
{

	[SerializeField]
	private EnnemiEnum ennemiType;
	
	private void Awake()
	{
		BoolEventSO boolEventSO = ScriptableObject.CreateInstance<BoolEventSO>();

		if (ennemiType == EnnemiEnum.Bandit)
		{
			gameObject.GetComponent<BanditBehaviour>().TakeDamageEvent = boolEventSO;
            gameObject.GetComponent<BanditBehaviour>().AddSuscribe();
            
            gameObject.GetComponent<LifeManagerEnnemi>().TakeDamageEventSO = boolEventSO;
            gameObject.GetComponent<BanditBehaviour>().TakeDamageEvent.Value = false;
		}
		
	}
}
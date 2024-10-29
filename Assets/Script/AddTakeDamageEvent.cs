using System;
using EventSystem.SO;
using UnityEngine;

public class AddTakeDamageEvent : MonoBehaviour
{
	private void Awake()
	{
		BoolEventSO boolEventSO = ScriptableObject.CreateInstance<BoolEventSO>();

		gameObject.GetComponent<BanditBehaviour>().TakeDamageEvent = boolEventSO;
		gameObject.GetComponent<BanditBehaviour>().AddSuscribe();
		gameObject.GetComponent<LifeManagerEnnemi>().TakeDamageEventSO = boolEventSO;
		gameObject.GetComponent<BanditBehaviour>().TakeDamageEvent.Value = false;
	}
}
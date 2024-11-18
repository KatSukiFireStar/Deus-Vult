using System;
using EventSystem.SO;
using UnityEngine;

public class CheckCollisionWithEnnemi : MonoBehaviour
{
	[SerializeField]
	private IntTriggerSO damageTrigger;
	
	[SerializeField] 
	private BoolEventSO hurtEvent;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Ennemi") || other.CompareTag("DemonSlime"))
		{
			damageTrigger.Trigger();
			hurtEvent.Value = true;
		}
	}
}
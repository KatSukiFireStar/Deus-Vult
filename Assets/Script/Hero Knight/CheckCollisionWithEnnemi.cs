using System;
using EventSystem.SO;
using UnityEngine;

/**
 * To damage the player when the collide with an enemy
 */
public class CheckCollisionWithEnnemi : MonoBehaviour
{
	[SerializeField]
	private IntTriggerSO damageTrigger;
	
	[SerializeField] 
	private BoolEventSO hurtEvent;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Ennemi") || other.CompareTag("DemonSlime") || other.CompareTag("BoD"))
		{
			damageTrigger.Trigger();
			hurtEvent.Value = true;
		}
	}
}
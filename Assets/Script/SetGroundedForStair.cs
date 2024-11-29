using System;
using EventSystem.SO;
using UnityEngine;

public class SetGroundedForStair : MonoBehaviour
{

	[SerializeField]
	private BoolEventSO groundedEvent;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("PlayerCollider"))
		{
			groundedEvent.Value = true;
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("PlayerCollider"))
		{
			groundedEvent.Value = false;
		}
	}
}
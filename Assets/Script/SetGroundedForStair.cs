using System;
using EventSystem.SO;
using UnityEngine;

public class SetGroundedForStair : MonoBehaviour
{

	[SerializeField]
	private BoolEventSO groundedEvent;
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerCollider"))
		{
			groundedEvent.Value = true;
		}
	}
	
	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerCollider"))
		{
			groundedEvent.Value = false;
		}
	}
}
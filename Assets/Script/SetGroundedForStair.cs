using System;
using EventSystem.SO;
using UnityEngine;

/*
 * Fix for player going through stairs, this is just a separate check to stop them from going through
 */
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
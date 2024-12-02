using System;
using EventSystem.SO;
using UnityEngine;

public class StartSlimeTrigger : MonoBehaviour
{
	[SerializeField] 
	private BoolEventSO startSlimeEvent;


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("PlayerCollider"))
		{
			startSlimeEvent.Value = true;
		}
	}
}
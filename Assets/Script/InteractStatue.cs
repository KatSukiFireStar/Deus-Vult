using System;
using EventSystem.SO;
using UnityEngine;

public class InteractStatue : MonoBehaviour
{
	[SerializeField] 
	private RespawnTriggerSO respawnTrigger;

	[SerializeField] 
	private BoolEventSO prayEvent;

	private void Awake()
	{
		prayEvent.Value = false;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if ((other.CompareTag("Player") || other.CompareTag("PlayerCollider")) && Input.GetKeyDown(KeyCode.E) && !prayEvent.Value)
		{
			prayEvent.Value = true;
		}
	}
}
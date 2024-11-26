using System;
using EventSystem.SO;
using UnityEngine;

public class InteractStatue : MonoBehaviour
{
	[SerializeField] 
	private RespawnTriggerSO respawnTrigger;

	[SerializeField] 
	private BoolEventSO prayEvent;

	
	private bool canPray = false;
	
	private void Awake()
	{
		prayEvent.Value = false;
	}

	private void Update()
	{
		if(!canPray)
			return;
		
		if (Input.GetKeyDown(KeyCode.E) && !prayEvent.Value)
		{
			prayEvent.Value = true;
			respawnTrigger.Trigger();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player") || other.CompareTag("PlayerCollider"))
			canPray = true;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Player") || other.CompareTag("PlayerCollider"))
			canPray = false;
	}
}
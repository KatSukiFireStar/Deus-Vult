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
		//if the player can't pray (not in the collider of the statue) remove all interaction
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
		//Check if the player enter on the statue collider and allow him to pray  and show the interaction touch
		if (other.CompareTag("Player") || other.CompareTag("PlayerCollider"))
		{
			canPray = true;
			transform.GetChild(1).gameObject.SetActive(true);
		}
			
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		//Check if the player go out off the statue collider and allow him to pray  and show the interaction touch
		if (other.CompareTag("Player") || other.CompareTag("PlayerCollider"))
		{
			canPray = false;
			transform.GetChild(1).gameObject.SetActive(false);
		}
			
	}
}
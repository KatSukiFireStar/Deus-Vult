using System;
using System.Collections.Generic;
using EventSystem.SO;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
	[SerializeField] 
	private GameObject player;
	
	[SerializeField] 
	private RespawnTriggerSO respawnTrigger;
	
	[SerializeField] 
	private List<BoolEventSO> eventToReset;

	private void Awake()
	{
		//If there is no gameObject with tag player it's the start of the game and we spawn him and reset event
		if (GameObject.FindGameObjectWithTag("Player") == null)
		{
			Instantiate(player, transform.position, Quaternion.identity);
			respawnTrigger.Trigger();
			foreach (BoolEventSO e in eventToReset)
			{
				e.Value = false;
			}
		}
	}
}
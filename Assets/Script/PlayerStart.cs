using System;
using System.Collections.Generic;
using EventSystem.SO;
using UnityEngine;

/*
 * Spawns the player on the first scene, will be disabled if we go back to the scene but the player is already spawned
 */
public class PlayerStart : MonoBehaviour
{
	[SerializeField] 
	private GameObject player;
	
	[SerializeField] 
	private RespawnTriggerSO respawnTrigger;
	
	[SerializeField] 
	private List<BoolEventSO> eventToReset;
	
	[SerializeField]
	private BoolsEventSO boolToReset;

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
			boolToReset.Value = new bool[4];
			for (int i = 0; i < boolToReset.Value.Length; i++)
			{
				boolToReset.Value[i] = false;
			}
		}
		Destroy(gameObject);
	}
}
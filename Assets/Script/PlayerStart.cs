using System;
using EventSystem.SO;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
	[SerializeField] 
	private GameObject player;
	
	[SerializeField] 
	private RespawnTriggerSO respawnTrigger;

	private void Awake()
	{
		if (GameObject.FindGameObjectWithTag("Player") == null)
		{
			Instantiate(player, transform.position, Quaternion.identity);
			respawnTrigger.Trigger();
		}
	}
}
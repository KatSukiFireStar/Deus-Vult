using System;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
	[SerializeField] 
	private GameObject player;

	private void Start()
	{
		if(GameObject.FindGameObjectWithTag("Player") == null)
			Instantiate(player, transform.position, Quaternion.identity);
	}
}
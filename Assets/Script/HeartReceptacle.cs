using System;
using EventSystem.SO;
using UnityEngine;

public class HeartReceptacle : MonoBehaviour
{
	[SerializeField] 
	private IntTriggerSO amountLifeToAdd;


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("PlayerCollider"))
		{
			amountLifeToAdd.Trigger();
			Destroy(gameObject);
		}
	}
}
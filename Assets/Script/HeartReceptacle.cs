using System;
using EventSystem.SO;
using UnityEngine;

public class HeartReceptacle : MonoBehaviour
{
	[SerializeField] 
	private IntTriggerSO amountLifeToAdd;

	[SerializeField] 
	private BoolEventSO heartPickupEvent;

	private void Start()
	{
		//if heart already pick up destroy them
		if (heartPickupEvent.Value)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//if player collide add the amount of life to the max life value and destroy gameObject
		if (other.CompareTag("PlayerCollider"))
		{
			amountLifeToAdd.Trigger();
			heartPickupEvent.Value = true;
			Destroy(gameObject);
		}
	}
}
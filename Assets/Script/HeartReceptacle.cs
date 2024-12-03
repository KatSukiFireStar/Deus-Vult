using System;
using EventSystem.SO;
using UnityEngine;

public class HeartReceptacle : MonoBehaviour
{
	[SerializeField] 
	private int number;
	
	[SerializeField] 
	private IntTriggerSO amountLifeToAdd;

	[SerializeField] 
	private BoolsEventSO heartPickupEvent;

	private void Start()
	{
		//if heart already pick up destroy them
		if (heartPickupEvent.Value[number])
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
			heartPickupEvent.Value[number] = true;
			Destroy(gameObject);
		}
	}
}
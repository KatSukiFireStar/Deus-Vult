using System;
using UnityEngine;

public class CheckEnterBoss : MonoBehaviour
{
	private enum Position
	{
		left,
		right
	}
	
	[SerializeField] 
	private Position positionExit;

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("PlayerCollider"))
		{
			if (positionExit == Position.left && other.transform.position.x < transform.position.x)
			{
				GetComponent<SpriteRenderer>().enabled = true;
				GetComponent<BoxCollider2D>().isTrigger = false;
			}else if (positionExit == Position.right && other.transform.position.x > transform.position.x)
			{
				GetComponent<SpriteRenderer>().enabled = true;
				GetComponent<BoxCollider2D>().isTrigger = false;
			}
		}
	}
}
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AttackManager : MonoBehaviour
{
	[SerializeField] 
	private int attackPoint;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Ennemi"))
		{
			other.gameObject.GetComponent<LifeManagerEnnemi>().LifeEvent.Value -= attackPoint;
		}else if (other.gameObject.CompareTag("DemonSlime"))
		{
			other.gameObject.GetComponent<LifeManagerDemonSlime>().LifeEvent.Value -= attackPoint;
		}
	}
}
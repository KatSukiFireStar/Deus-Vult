using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider2D))]
public class AttackManager : MonoBehaviour
{
	[SerializeField] 
	private int attackPoint;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		//Manage the attack of the player and apply the damage to the right event of the other collider
		
		if (other.gameObject.CompareTag("Ennemi"))
		{
			other.gameObject.GetComponent<LifeManagerEnnemi>().LifeEvent.Value -= attackPoint;
		}else if (other.gameObject.CompareTag("DemonSlime"))
		{
			other.gameObject.GetComponent<LifeManagerDemonSlime>().LifeEvent.Value -= attackPoint;
		}else if (other.gameObject.CompareTag("BoD"))
		{
			other.gameObject.GetComponent<LifeManagerBoD>().LifeEvent.Value -= attackPoint;
		}
	}
}
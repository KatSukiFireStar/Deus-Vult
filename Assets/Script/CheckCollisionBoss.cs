using System;
using System.Collections.Generic;
using EventSystem.SO;
using UnityEngine;

public class CheckCollisionBoss : MonoBehaviour
{
	[SerializeField] 
	private List<IntTriggerSO> damageTriggers;
	
	private Animator m_animator;

	private void Start()
	{
		m_animator = transform.parent.GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//if the player collide with the collider set the damage to the player event
		if (other.gameObject.CompareTag("Player"))
		{
			int i = m_animator.GetInteger("AttackNb");
			damageTriggers[i].Trigger();
		}
	}
}
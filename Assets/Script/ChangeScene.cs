using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	[SerializeField] 
	private RespawnTriggerSO changeSceneTrigger;
	
	private Animator animator;

	private void Start()
	{
		animator = GameObject.FindGameObjectWithTag("Player").transform.GetComponentsInChildren<Animator>()[1];
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//If the player collide with the trigger, use a trigger in order to change is next position and do a fade out
		if (other.CompareTag("PlayerCollider"))
		{
			animator.SetTrigger("FadeOut");
			changeSceneTrigger.Trigger();
		}
		
	}
}
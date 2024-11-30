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
		if (other.CompareTag("PlayerCollider"))
		{
			Debug.Log("Je change de scene pour scene: " + changeSceneTrigger.modifier.sceneName);
			animator.SetTrigger("FadeOut");
			changeSceneTrigger.Trigger();
		}
		
	}
}
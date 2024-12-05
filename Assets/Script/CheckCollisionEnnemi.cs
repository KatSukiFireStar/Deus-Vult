
using System;
using EventSystem.SO;
using UnityEngine;

public class CheckCollisionEnnemi : MonoBehaviour
{
    [SerializeField] 
    private IntTriggerSO damageTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the player collide with an ennemi attack collider and apply the damage to the player event
        if (other.gameObject.CompareTag("Player"))
        {
            damageTrigger.Trigger();
        }
    }
}

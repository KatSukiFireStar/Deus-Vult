
using System;
using EventSystem.SO;
using UnityEngine;

public class CheckCollisionEnnemi : MonoBehaviour
{
    [SerializeField] 
    private IntTriggerSO damageTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            damageTrigger.Trigger();
        }
    }
}

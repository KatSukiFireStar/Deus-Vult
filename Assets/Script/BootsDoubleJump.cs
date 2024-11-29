using System.Collections;
using System.Collections.Generic;
using EventSystem.SO;
using UnityEngine;

public class BootsDoubleJump : MonoBehaviour
{
    [SerializeField] private BoolTriggerSO _bootPickupEvent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _bootPickupEvent.Trigger();
            Destroy(gameObject);
        }
    }
}
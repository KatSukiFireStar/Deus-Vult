using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem.SO;
using UnityEngine;

public class BootsDoubleJump : MonoBehaviour
{
    [SerializeField] private BoolTriggerSO _bootPickupEvent;

    private void Start()
    {
        //if boot already pick up destroy them
        if (_bootPickupEvent.events.Value)
        {
            Debug.LogError("Je detruis les bottes");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if player collide add double jump and destroy gameObject
        if (other.CompareTag("PlayerCollider"))
        {
            Debug.LogError("Je prend les bottes");
            _bootPickupEvent.Trigger();
            Destroy(gameObject);
        }
    }
}
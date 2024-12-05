using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem.SO;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [SerializeField] private IntTriggerSO _healingTrigger;

    [SerializeField] private IntEventSO _playerLifeEvent;

    [SerializeField] private IntEventSO _maxLifeEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If the player collide heal him and destoy apple
        if (other.CompareTag("PlayerCollider"))
        {
            _healingTrigger.Trigger();
            Destroy(gameObject);
        }
    }
}
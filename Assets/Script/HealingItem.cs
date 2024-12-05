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
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            _healingTrigger.Trigger();
            Destroy(gameObject);
        }
    }
}
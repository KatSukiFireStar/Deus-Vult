﻿using UnityEngine;
using System.Collections;

/*
 * For physics, to slide on walls and check if grounded
 */
public class Sensor_HeroKnight : MonoBehaviour {

    private int m_ColCount = 0;

    private float m_DisableTimer;

    private void OnEnable()
    {
        m_ColCount = 0;
    }

    public bool State()
    {
        if (m_DisableTimer > 0)
            return false;
        return m_ColCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Trigger") && !other.CompareTag("Ennemi") && !other.CompareTag("DemonSlime"))
            m_ColCount++;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Trigger") && !other.CompareTag("Ennemi") && !other.CompareTag("DemonSlime"))
            m_ColCount--;
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }
}

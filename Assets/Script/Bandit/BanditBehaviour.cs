using System;
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using EventSystem.SO;

public class BanditBehaviour : MonoBehaviour
{
    [SerializeField]
    float m_speed = 4.0f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private SpriteRenderer m_spriteRenderer;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private bool m_attack = false;
    private bool m_isAttacking = false;
    private float inputX = -1;
    private float saveInputX;
    private bool m_takeDamage = false;
    private bool m_isHurt = false;
    private bool m_dying = false;

    [SerializeField]
    private int minX;

    [SerializeField]
    private int maxX;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private BoolEventSO takeDamageEvent;

    public BoolEventSO TakeDamageEvent
    {
        get => takeDamageEvent;
        set => takeDamageEvent = value;
    }
    
    [SerializeField] 
    private BoolEventSO deadEvent;

    public BoolEventSO DeadEvent
    {
        get => deadEvent;
        set => deadEvent = value;
    }

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_grounded = true;
        m_animator.SetBool("Grounded", m_grounded);
        saveInputX = inputX;
    }

    public void AddSuscribe()
    {
        takeDamageEvent.PropertyChanged += TakeDamageEventOnPropertyChanged;
    }

    public void AddSuscribeDead()
    {
        deadEvent.PropertyChanged += DeadEventOnPropertyChanged;
    }

    private void OnDestroy()
    {
        deadEvent.PropertyChanged -= DeadEventOnPropertyChanged;
        takeDamageEvent.PropertyChanged -= TakeDamageEventOnPropertyChanged;
    }

    private void DeadEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
        m_isDead = true;
    }

    private void TakeDamageEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
        m_takeDamage = s.Value;
    }

    void Update()
    {
        if (m_dying)
            return;
        
        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            m_spriteRenderer.flipX = true;
            transform.GetChild(1).localScale = new(-1, 1, 1);
        }
        else if (inputX < 0)
        {
            m_spriteRenderer.flipX = false;
            transform.GetChild(1).localScale = new(1, 1, 1);
        }

        if ((m_body2d.position.x < minX && inputX < 0) || (m_body2d.position.x > maxX && inputX > 0))
        {
            inputX *= -1;
            if(inputX != 0)
                saveInputX = inputX;
        }

        // Move
        if(!m_isHurt)
            m_body2d.velocity = new Vector2(inputX * m_speed, 0);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(m_body2d.position, Vector2.one * 0.75f, 0f, new(saveInputX, 0), 2.5f,
            layerMask);
        bool hitb = false;
        bool attackb = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                hitb = true;
                if (hit.distance < 0.5f)
                {
                    attackb = true;
                }
            }
        }

        if (!m_isAttacking && !m_isHurt)
        {
            if (hitb && !attackb)
            {
                m_combatIdle = true;
                inputX = 0;
            }
            else if (attackb)
            {
                m_attack = true;
                inputX = 0;
            }
            else
            {
                m_combatIdle = false;
                inputX = saveInputX;
            }
        }


        //Death
        if (m_isDead)
        {
            m_animator.SetTrigger("Death");
            m_body2d.velocity = new Vector2(0, 0);
            m_dying = true;
        }

        //Hurt
        else if (m_takeDamage && !m_isHurt)
        {
            m_animator.SetTrigger("Hurt");
            m_isHurt = true;
            m_body2d.velocity = new Vector2(-saveInputX * m_speed, 0);
        }
            

        //Attack
        else if (m_attack && !m_isAttacking)
        {
            m_animator.SetTrigger("Attack");
            m_isAttacking = true;
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    public void EndAttacking()
    {
        m_isAttacking = false;
        m_attack = false;
    }

    public void EndHurting()
    {
        takeDamageEvent.Value = false;
        m_isHurt = false;
        inputX = saveInputX;
        m_body2d.velocity = new Vector2(inputX * m_speed, 0);
    }

    public void EndDeath()
    {
        Destroy(gameObject);
    }
}
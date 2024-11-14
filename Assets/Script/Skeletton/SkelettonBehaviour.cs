using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

public class SkelettonBehaviour : MonoBehaviour
{
    [SerializeField]
    float m_speed = 4.0f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private SpriteRenderer m_spriteRenderer;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private bool m_attackA = false;
    private bool m_attackB = false;
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
    
    // Start is called before the first frame update
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

    private void DeadEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        m_isDead = true;
    }

    private void TakeDamageEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
        m_takeDamage = s.Value;
    }
    
    
    // Update is called once per frame
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
        
        //RayCasting with a layerMask set to players. This tells us if we hit a player and the distance.
        RaycastHit2D[] hits = Physics2D.BoxCastAll(m_body2d.position, Vector2.one * 0.75f, 0f, new(saveInputX, 0), 2.5f,
            layerMask);
        
        bool hitb = false;
        bool attackb = false;
        bool moveTowards = false;
        
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                hitb = true;
                if (hit.distance < 0.5f)
                {
                    attackb = true; // We're close enough to attack
                }
                else if (hit.distance < 2f) 
                {
                    moveTowards = true; // We're in the chasing range
                }
            }
        }
    }
}

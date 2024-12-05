using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;
using Random = UnityEngine.Random;

public class DemonSlimeBehaviour : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    float m_speed = 1.0f;
    [SerializeField]
    private int minX;
    [SerializeField]
    private int maxX;
    [SerializeField] 
    private float deltaAttack;
    
    [Header("Collision layer")]
    [SerializeField]
    private LayerMask layerMask;
    
    [Header("Events")]
    [SerializeField] 
    private BoolEventSO takeDamageEventSO;
    [SerializeField]
    private BoolEventSO deadEventSO;
    [SerializeField]
    private BoolEventSO transformEventSO;
    [SerializeField] 
    private BoolEventSO startEventSO;
    

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private SpriteRenderer m_spriteRenderer;
    private bool m_isDead = false;
    private bool m_attack = false;
    private bool m_isAttacking = false;
    private float inputX = 0;
    private float saveInputX;
    private bool m_takeDamage = false;
    private bool m_isHurt = false;
    private bool m_dying = false;
    private bool m_transform = false;
    private bool m_isTransforming = false;
    private bool m_start = false;
    private int m_numAttack = -1;
    private float m_lastAttack;
    
    private GameObject m_player;
    
    private void Awake()
    {
        takeDamageEventSO.PropertyChanged += TakeDamageEventSOOnPropertyChanged;
        deadEventSO.PropertyChanged += DeadEventSOOnPropertyChanged;
        transformEventSO.PropertyChanged += TransformEventSOOnPropertyChanged;
        startEventSO.PropertyChanged += StartEventSOOnPropertyChanged;

        m_lastAttack = deltaAttack;
    }

    private void OnDestroy()
    {
        takeDamageEventSO.PropertyChanged -= TakeDamageEventSOOnPropertyChanged;
        deadEventSO.PropertyChanged -= DeadEventSOOnPropertyChanged;
        transformEventSO.PropertyChanged -= TransformEventSOOnPropertyChanged;
        startEventSO.PropertyChanged -= StartEventSOOnPropertyChanged;
    }

    private void StartEventSOOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>) sender;
        m_start = s.Value;
        if (s.Value)
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
            inputX = -1;
            saveInputX = inputX;
            m_transform = false;
            EndAttacking();
            EndHurting();
        }
    }

    private void TransformEventSOOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>) sender;
        m_transform = s.Value;
        m_isTransforming = s.Value;
        if (s.Value)
        {
            inputX = 0;
            m_body2d.velocity = new Vector2(inputX * m_speed, 0);
            m_animator.SetBool("Transform", true);
            m_animator.SetTrigger("Transformation");
        }
    }

    private void DeadEventSOOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>) sender;
        m_animator.SetTrigger("Death");
        m_body2d.velocity = new Vector2(0, 0);
        m_dying = true;
    }

    private void TakeDamageEventSOOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>) sender;
        m_takeDamage = s.Value;
    }

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!m_start || m_dying || m_isTransforming)
            return;

        if (!m_transform)
        {
            if ((m_body2d.position.x < minX && inputX < 0) || (m_body2d.position.x > maxX && inputX > 0))
            {
                inputX *= -1;
                if(inputX != 0)
                    saveInputX = inputX;
            }
        }
        else
        {
            if (!m_isHurt && !m_isAttacking && m_player.transform.position.x < transform.position.x)
            {
                inputX = -1;
            }
            else if (!m_isHurt && !m_isAttacking && m_player.transform.position.x > transform.position.x)
            {
                inputX = 1;
            }
            if(inputX != 0)
                saveInputX = inputX;
        }

        if (m_lastAttack > 0 && !m_isAttacking)
        {
            m_lastAttack -= Time.deltaTime;
        }
        
        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            m_spriteRenderer.flipX = true;
            for (int i = 2; i < transform.childCount; i++)
                transform.GetChild(i).localScale = new(-1, 1, 1);
        }
        else if (inputX < 0)
        {
            m_spriteRenderer.flipX = false;
            for (int i = 2; i < transform.childCount; i++)
                transform.GetChild(i).localScale = new(1, 1, 1);
        }
        
        // Move
        if (!m_isHurt)
            m_body2d.velocity = new Vector2(inputX * m_speed, 0);
        else if(m_isAttacking)
            m_body2d.velocity = new Vector2(0, 0);
        else if (m_isHurt)
            m_body2d.velocity = new Vector2(-saveInputX * m_speed, 0);
            
        //If the slime isn't transformed we stop it
        //The 1st phase can only walk from two point and take damage
        if (!m_transform)
        {
            //Contextual animation
            //Damage Anim
            if (m_takeDamage && !m_isHurt)
            {
                m_animator.SetTrigger("Hit");
                inputX = 0;
                m_isHurt = true;
                
            }
            //Move Anim
            else if (Mathf.Abs(inputX) > Mathf.Epsilon)
                m_animator.SetInteger("AnimState", 1);
            
            return;
        }
        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(m_body2d.position, Vector2.one * 0.75f, 0f, new(saveInputX, 0), 5f,
            layerMask);
        float dist = -1;
        bool attackb = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (m_numAttack != -1 || m_lastAttack > 0f)
                    break;
                
                dist = hit.distance;
                
                if (hit.distance <= 1f)
                {
                    m_numAttack = Random.Range(0, 3);
                }
                else if (hit.distance <= 5f)
                {
                   m_numAttack = Random.Range(0, 4);

                   if (m_numAttack == 3 && saveInputX > 0)
                   {
                       m_numAttack = 2;
                   }
                }
                m_animator.SetInteger("AttackNb", m_numAttack);
            }
        }
        
        if (!m_isAttacking)
        {
            if (m_numAttack >= 2)
            {
                m_attack = true;
            }
            else if (m_numAttack >= 0)
            {
                if (dist <= 0.5f)
                {
                    m_attack = true;
                }
            }
        }
        
        
        
        //Attack
        if (m_attack && !m_isAttacking)
        {
            m_animator.SetTrigger("Attack");
            inputX = 0;
            m_isAttacking = true;
        }
        
        //Hurt
        else if (m_takeDamage && !m_isHurt && !m_isAttacking)
        {
            m_animator.SetTrigger("Hit");
            inputX = 0;
            m_isHurt = true;
            m_body2d.velocity = new Vector2(-saveInputX * m_speed, 0);
        }
        
        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }
    
    public void EndAttacking()
    {
        m_isAttacking = false;
        m_attack = false;
        m_numAttack = -1;
        inputX = saveInputX;
        m_body2d.velocity = new Vector2(inputX * m_speed, 0);
        m_lastAttack = deltaAttack;
    }
    
    public void EndHurting()
    {
        inputX = saveInputX;
        takeDamageEventSO.Value = false;
        m_isHurt = false;
        m_body2d.velocity = new Vector2(inputX * m_speed, 0);
    }
    
    public void EndTransforming()
    {
        m_isTransforming = false;
        m_transform = true;
        transform.GetChild(1).gameObject.SetActive(true);
        // GetComponent<LifeManagerDemonSlime>().ReSuscribeLife();
    }

    public void EndDeath()
    {
        Destroy(gameObject);
    }
}

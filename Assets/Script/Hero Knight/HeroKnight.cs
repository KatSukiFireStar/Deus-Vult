using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using EventSystem.SO;

public class HeroKnight : MonoBehaviour
{
    [SerializeField]
    float m_speed = 4.0f;

    [SerializeField]
    float m_jumpForce = 6f;

    [SerializeField]
    float m_rollForce = 6.0f;

    [SerializeField]
    bool m_noBlood = false;

    [SerializeField]
    GameObject m_slideDust;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private bool m_takeDamage = false;
    private bool m_isHurt = false;
    private bool m_isPraying = false;
    private bool m_isDead = false;
    private bool m_dying = false;
    private bool m_attacking = false;
    private bool m_pray = false;
    private bool m_endPray = false;
    private bool m_canMove = true;
    private bool m_secondJump = false;
    private bool m_hasBoots = false;
    private bool m_canRoll = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;

    private List<KeyCode> m_inputs = new();
    
    [Header("Events")]
    [SerializeField]
    private BoolEventSO hurtEvent;
    
    [SerializeField] 
    private BoolEventSO deathEvent;
    
    [SerializeField] 
    private BoolEventSO respawnEvent;
    
    [SerializeField] 
    private BoolEventSO fadeEvent;

    [SerializeField]
    private BoolEventSO prayEvent;

    [SerializeField] 
    private BoolEventSO bootPickupEvent;
    
    [SerializeField]
    private BoolEventSO groundedEvent;
    
    public BoolEventSO HurtEvent
    { 
        get => hurtEvent;
        set => hurtEvent = value;
    }

    private void Awake()
    {
        hurtEvent.PropertyChanged += HurtEventOnPropertyChanged;
        deathEvent.PropertyChanged += DeathEventOnPropertyChanged;
        fadeEvent.PropertyChanged += FadeEventOnPropertyChanged;
        prayEvent.PropertyChanged += PrayEventOnPropertyChanged;
        bootPickupEvent.PropertyChanged += BootPickupEventOnPropertyChanged;
        
        m_inputs.Add(KeyCode.A);
        m_inputs.Add(KeyCode.D);
        m_inputs.Add(KeyCode.W);
        m_inputs.Add(KeyCode.S);
        m_inputs.Add(KeyCode.Space);
        m_inputs.Add(KeyCode.Z);
        m_inputs.Add(KeyCode.Q);
    }

    private void OnDestroy()
    {
        hurtEvent.PropertyChanged -= HurtEventOnPropertyChanged;
        deathEvent.PropertyChanged -= DeathEventOnPropertyChanged;
        fadeEvent.PropertyChanged -= FadeEventOnPropertyChanged;
        prayEvent.PropertyChanged -= PrayEventOnPropertyChanged;
        bootPickupEvent.PropertyChanged -= BootPickupEventOnPropertyChanged;
    }

    private void PrayEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
        m_pray = s.Value;
    }

    private void FadeEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
        if (s.Value)
        {
            m_animator.SetTrigger("Idle");
            EndAttacking();
            EndHurting();
            EndRolling();
            EndDeath();
        }
            
    }

    private void DeathEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
        m_isDead = s.Value;
    }

    private void HurtEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
        m_takeDamage = s.Value;
    }

    private void BootPickupEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>)sender;
        m_hasBoots = s.Value;
    }

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_pray && m_isPraying && m_endPray)
        {
            bool goOut = false;
            foreach (KeyCode key in m_inputs)
            {
                if (Input.GetKey(key))
                {
                    goOut = true;
                }
            }

            if (!goOut)
                return;
            m_animator.SetTrigger("EndPray");
            m_isPraying = false;
            m_endPray = false;
            prayEvent.Value = false;
        }
        
        if (m_dying || !m_canMove)
            return;
        
        
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
        {
            m_rolling = false;
            m_rollCurrentTime = 0;
        }

        //Check if character just landed on the ground
        if ((!m_grounded && m_groundSensor.State()) || groundedEvent.Value)
        {
            m_grounded = true;
            m_secondJump = false;
            m_animator.SetBool("Grounded", m_grounded);
            if (groundedEvent.Value)
            {
                m_canRoll = false;
            }
            else
            {
                m_canRoll = true;
            }
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State() && !groundedEvent.Value)
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            transform.GetChild(1).localScale = new(1, 1, 1);
            m_facingDirection = 1;
        }

        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.GetChild(1).localScale = new(-1, 1, 1);
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling && !m_isHurt && !m_attacking)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
        else if (m_isHurt)
            m_body2d.velocity = new Vector2(-m_facingDirection * m_speed * 2, m_body2d.velocity.y);
        else if(m_attacking && m_grounded)
            m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);
        else if (m_rolling)
        {
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce * m_speed, m_body2d.velocity.y);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) ||
                          (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        if (m_isDead)
        {
            EndHurting();
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
            m_dying = true;
            m_body2d.velocity = new Vector2(0, 0);
        }
        
        //Pray
        else if (m_pray && !m_isPraying && m_grounded)
        {
            m_animator.SetTrigger("Pray");
            m_isPraying = true;
            m_canMove = false;
            m_body2d.velocity = new Vector2(0, 0);
        }

        //Hurt
        else if (m_takeDamage && !m_isHurt && !m_rolling)
        {
            m_animator.SetTrigger("Hurt");
            m_isHurt = true;
        }
        
        // Roll
        else if (Input.GetKeyDown("left shift") && m_canRoll && !m_rolling && !m_isWallSliding && m_grounded && !m_attacking && !m_isHurt && !m_isPraying)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.excludeLayers |= (1 << 7);
        }
        
        //Attack
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;
            m_attacking = true;
            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Useless for the moment maybe it'll be developed later
        // Block
        // else if (Input.GetMouseButtonDown(1) && !m_rolling)
        // {
        //     m_animator.SetTrigger("Block");
        //     m_animator.SetBool("IdleBlock", true);
        // }
        //
        // else if (Input.GetMouseButtonUp(1))
        //     m_animator.SetBool("IdleBlock", false);
        
        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
            groundedEvent.Value = false;
        }
        
        
        //Second jump if possible
        else if (Input.GetKeyDown("space") && !m_grounded && !m_rolling && m_hasBoots && !m_secondJump) // If we have boots and haven't performed a second jump yet
        {
            m_secondJump = true;
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        }
        

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    public void EndAttacking()
    {
        m_attacking = false;
    }

    public void EndRolling()
    {
        m_rolling = false;
        m_body2d.excludeLayers = default;
    }
    
    public void EndHurting()
    {
        m_isHurt = false;
        hurtEvent.Value = false;
    }

    public void EndDeath()
    {
        deathEvent.Value = false;
        m_isDead = false;
        m_dying = false;
    }

    public void EndPray()
    {
        m_endPray = true;
    }

    public void RestartMove()
    {
        m_canMove = true;
    }

    public void StartRespawn()
    {
        respawnEvent.Value = true;
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
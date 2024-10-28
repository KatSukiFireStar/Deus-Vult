using System;
using UnityEngine;
using System.Collections;

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

    [SerializeField]
    private int minX;

    [SerializeField]
    private int maxX;

    [SerializeField]
    private LayerMask layerMask;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_grounded = true;
        m_animator.SetBool("Grounded", m_grounded);
        saveInputX = inputX;
    }

    void Update()
    {
        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            m_spriteRenderer.flipX = true;
        else if (inputX < 0)
            m_spriteRenderer.flipX = false;

        if ((m_body2d.position.x < minX && inputX < 0) || (m_body2d.position.x > maxX && inputX > 0))
        {
            inputX *= -1;
            if(inputX != 0)
                saveInputX = inputX;
        }

        // Move
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

        if (!m_isAttacking)
        {
            if (hitb && !attackb)
            {
                m_combatIdle = true;
                inputX = 0;
            }
            else if (attackb)
            {
                m_attack = true;
            }
            else
            {
                m_combatIdle = false;
                inputX = saveInputX;
            }
        }


        //Death
        if (Input.GetKeyDown("e"))
        {
            if (!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }

        //Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");

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
}
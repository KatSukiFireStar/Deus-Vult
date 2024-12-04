using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using EventSystem.SO;

public class BoDBehavior : MonoBehaviour
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
    private BoolEventSO startEventSO;
    

    private Animator _animator;
    private Rigidbody2D _body2d;
    private SpriteRenderer _spriteRenderer;
    private bool _isDead = false;
    private bool _attack = false;
    private bool _isAttacking = false;
    private float _inputX = 0;
    private float _saveInputX;
    private float _deltaAttacked;
    private bool _takeDamage = false;
    private bool _isHurt = false;
    private bool _dying = false;
    private bool _start = false;
    private bool _casting = false;
    private bool _chasing = false;
    
    private GameObject _player;
    
    private void Awake()
    {
        takeDamageEventSO.PropertyChanged += TakeDamageEventSOOnPropertyChanged;
        deadEventSO.PropertyChanged += DeadEventSOOnPropertyChanged;
        startEventSO.PropertyChanged += StartEventSOOnPropertyChanged;

        _deltaAttacked = deltaAttack;
    }

    private void StartEventSOOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>) sender;
        _start = s.Value;
        if (s.Value)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _inputX = -1;
            _saveInputX = _inputX;
        }
    }

    private void DeadEventSOOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>) sender;
        _dying = s.Value;
    }

    private void TakeDamageEventSOOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<bool> s = (GenericEventSO<bool>) sender;
        _takeDamage = s.Value;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _body2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // On ne fait rien si on est mort, mourant ou que l'on a pas démarré
        if (!_start || _dying || _isDead)
            return;

        if (_chasing)
        {
            if ((_body2d.position.x < minX && _inputX < 0) || (_body2d.position.x > maxX && _inputX > 0))
            {
                _inputX *= -1;
                if(_inputX != 0)
                    _saveInputX = _inputX;
            }
        }
        else
        {
            if (!_isHurt && !_isAttacking && _player.transform.position.x < transform.position.x)
            {
                _inputX = -1;
            }
            else if (!_isHurt && !_isAttacking && _player.transform.position.x > transform.position.x)
            {
                _inputX = 1;
            }
            if(_inputX != 0)
                _saveInputX = _inputX;
        }

        if (!_isAttacking && _deltaAttacked > 0)
        {
            _deltaAttacked -= Time.deltaTime;
            if (_deltaAttacked < 0)
            {
                _deltaAttacked = deltaAttack;
                _attack = true;
            }
        }
        
        // Swap direction of sprite depending on walk direction
        if (_inputX > 0)
        {
            _spriteRenderer.flipX = true;
            for (int i = 2; i < transform.childCount; i++)
                transform.GetChild(i).localScale = new(-1, 1, 1);
        }
        else if (_inputX < 0)
        {
            _spriteRenderer.flipX = false;
            for (int i = 2; i < transform.childCount; i++)
                transform.GetChild(i).localScale = new(1, 1, 1);
        }
        
        // Move
        if (!_isHurt)
            _body2d.velocity = new Vector2(_inputX * m_speed, 0);
        else if(_isAttacking)
            _body2d.velocity = new Vector2(0, 0);
        else if (_isHurt)
            _body2d.velocity = new Vector2(-_saveInputX * m_speed, 0);
        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(_body2d.position, Vector2.one * 0.75f, 0f, new(_saveInputX, 0), 5f,
            layerMask);
        float dist = -1;
        bool attackb = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                dist = hit.distance;
                
                if (hit.distance <= 1f)
                {
                    _attack = true;
                }
                else if (hit.distance <= 5f)
                {
                    _attack = false;
                    _chasing = true;
                }
            }
        }
        
        
        
        //Death
        if (_isDead)
        {
            _animator.SetTrigger("Death");
            _body2d.velocity = new Vector2(0, 0);
            _dying = true;
        }
        
        //Attack
        else if (_attack && !_isAttacking)
        {
            _animator.SetTrigger("Attack");
            _inputX = 0;
            _isAttacking = true;
        }
        
        //Hurt
        else if (_takeDamage && !_isHurt && !_isAttacking)
        {
            _animator.SetTrigger("Hurt");
            _inputX = 0;
            _isHurt = true;
            _body2d.velocity = new Vector2(-_saveInputX * m_speed, 0);
        }
        
        //Run
        else if (Mathf.Abs(_inputX) > Mathf.Epsilon)
            _animator.SetInteger("AnimState", 1);

        //Idle
        else
            _animator.SetInteger("AnimState", 0);
    }
    
    public void EndAttacking()
    {
        _isAttacking = false;
        _attack = false;
        _inputX = _saveInputX;
        _body2d.velocity = new Vector2(_inputX * m_speed, 0);
        _deltaAttacked = deltaAttack;
    }
    
    public void EndHurting()
    {
        _inputX = _saveInputX;
        takeDamageEventSO.Value = false;
        _isHurt = false;
        _body2d.velocity = new Vector2(_inputX * m_speed, 0);
    }
}

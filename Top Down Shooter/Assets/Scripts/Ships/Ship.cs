using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ship : MonoBehaviour
{
    [Header("Movement")]
    public GameObject ShipObject = null;

    [SerializeField]
    protected float _moveSpeed = 3.5f;

    [SerializeField]
    protected float _rotateSpeed = 2f;

    [Header("Health")]
    [SerializeField]
    private float _maxHealth = 100f;

    private float _currentHealth = -1f;

    [SerializeField]
    private Slider _healthBar = null;

    [Header("Damage")]
    public float DamageOther = 25f;

    [SerializeField]
    private Sprite[] _shipSprites = null;

    private int _spriteInUse = 0;

    protected Animator _animator;

    protected Rigidbody2D _rigidbody;

    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
        _healthBar.value = _currentHealth;

        _spriteInUse = 0;
        ShipObject.GetComponent<SpriteRenderer>().sprite = _shipSprites[_spriteInUse];

        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            Debug.LogError("Ship Rigidbody2D is NULL");
        }

        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Ship animator is NULL");
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {

    }

    public void TakeDamage(float damageReceived)
    {
        _currentHealth -= damageReceived;

        _healthBar.value = _currentHealth;

        if(_currentHealth <= 0)
        {
            _animator.SetTrigger("Explode");
            return;
        }

        _spriteInUse++;

        ShipObject.GetComponent<SpriteRenderer>().sprite = _shipSprites[_spriteInUse];

        _animator.SetTrigger("Break");
    }

    protected virtual void Death()
    {

    }
}

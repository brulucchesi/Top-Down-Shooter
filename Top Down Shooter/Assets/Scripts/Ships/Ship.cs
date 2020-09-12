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

    private Rect _cameraBounds;

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
        _cameraBounds = GameManager.Instance.GetCameraBounds();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        MoveBounds();
        Move();
    }

    protected virtual void Move()
    {

    }

    private void MoveBounds()
    {
        Vector3 shipSize = ShipObject.GetComponent<SpriteRenderer>().bounds.extents;

        Vector3 pos = transform.position;

        pos.y = Mathf.Clamp(transform.position.y, _cameraBounds.yMin + shipSize.y, _cameraBounds.yMax - shipSize.y);

        pos.x = Mathf.Clamp(transform.position.x, _cameraBounds.xMin + shipSize.x, _cameraBounds.xMax - shipSize.x);

        transform.position = pos;
    }

    public void TakeDamage(float damageReceived)
    {
        _currentHealth -= damageReceived;

        _healthBar.value = _currentHealth;

        if (_currentHealth <= 0)
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


    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 2));
    }
}

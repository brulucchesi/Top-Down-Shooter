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

    protected float _currentHealth = -1f;

    [SerializeField]
    private Slider _healthBar = null;

    private float _healthAmountToChangeSprite;

    [Header("Damage")]
    public float DamageOther = 25f;

    [SerializeField]
    private Sprite[] _shipSprites = null;

    [SerializeField]
    private GameObject _explosion = null;

    private int _spriteInUse = 0;

    protected Animator _animator;

    protected Rigidbody2D _rigidbody;

    protected Rect _cameraBounds;

    protected GameManager _gameManager;

    protected AudioManager _audioManager;

    protected virtual void Start()
    {
        _healthBar.maxValue = _maxHealth;

        _currentHealth = _maxHealth;

        _healthAmountToChangeSprite = _maxHealth / _shipSprites.Length;

        UpdateHealth();

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

        _gameManager = GameManager.Instance;
        _audioManager = AudioManager.Instance;

        _cameraBounds = _gameManager.GetCameraBounds();
    }
    
    protected virtual void FixedUpdate()
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

        if (_currentHealth <= 0)
        {
            EnableShipCollider(false);

            _audioManager.PlayExplosionSFX();

            _animator.SetTrigger("Explode");
            return;
        }

        UpdateHealth();
        
        float healthLimitToChangeSprite = _maxHealth - _healthAmountToChangeSprite * (_spriteInUse + 1);

        if(_currentHealth <= healthLimitToChangeSprite)
            ChangeSprite(++_spriteInUse);

        _animator.SetTrigger("Break");
    }

    private void UpdateHealth()
    {
        _healthBar.value = _currentHealth;
    }

    private void ChangeSprite(int index)
    {
        ShipObject.GetComponent<SpriteRenderer>().sprite = _shipSprites[index];
    }

    protected virtual void Death()
    {
        
    }

    protected virtual void EnableShipCollider(bool enable)
    {
        Collider2D col = ShipObject.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = enable;
        }
    }

    public virtual void ResetShip()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        _currentHealth = _maxHealth;
        UpdateHealth();

        _explosion.SetActive(false);

        _spriteInUse = 0;

        ChangeSprite(_spriteInUse);
    }
}

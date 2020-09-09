using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private GameObject _ship = null;

    [SerializeField]
    private float _moveSpeed = 200.0f;

    [SerializeField]
    private float _rotateSpeed = 200.0f;
    
    [Header("Health")]
    private Slider _healthBar;

    [SerializeField]
    private int _maxHealth = 100;

    [Header("Shoot")]
    [SerializeField]
    private GameObject _bulletPrefab = null;

    [SerializeField]
    private float _shootRate = 0.15f;
    private float _canShoot = -1f;

    [SerializeField]
    private float _bulletYOffset = 40f;

    [SerializeField]
    private float _bulletXOffset = 40f;

    private Animator _animator;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();

        if (_rigidbody == null)
        {
            Debug.LogError("Player Rigidbody2D is NULL");
        }

        _animator = gameObject.GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Player animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canShoot)
        {
            FrontalShoot();
        }
    }

    private void FrontalShoot()
    {
        _canShoot = Time.time + _shootRate;

        Instantiate(_bulletPrefab, transform.position, Quaternion.identity, transform.parent);
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _rigidbody.velocity = -_ship.transform.up * verticalInput * _moveSpeed;

        float rotation = -horizontalInput * _rotateSpeed;

        _ship.transform.Rotate(Vector3.forward * rotation);
    }
}

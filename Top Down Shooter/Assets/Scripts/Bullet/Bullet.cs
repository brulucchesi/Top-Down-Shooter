using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _lifetimeInSeconds = 2f;

    private float _damage;

    private bool _isPlayerShooting = false;

    private Rigidbody2D _rigidbody;

    private Animator _animator;

    private bool _canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        _canMove = true;

        _rigidbody = gameObject.GetComponent<Rigidbody2D>();

        if (_rigidbody == null)
        {
            Debug.LogError("Bullet Rigidbody2D is NULL");
        }

        _animator = gameObject.GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Bullet Animator is NULL");
        }

        StartCoroutine(LifetimeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!_canMove)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        _rigidbody.velocity = -transform.up * _moveSpeed;
    }

    public void AssignDamage(float damage)
    {
        _damage = damage;
    }

    public void AssignBulletToPlayer()
    {
        _isPlayerShooting = true;
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(_lifetimeInSeconds);

        _canMove = false;
        _animator.SetTrigger("Explode");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Player" && !_isPlayerShooting) || (other.tag == "Enemy" && _isPlayerShooting))
        {
            _canMove = false;

            Ship ship = other.transform.parent.GetComponent<Ship>();

            if (ship != null)
            {
                ship.TakeDamage(_damage);
            }

            _animator.SetTrigger("Explode");
        }
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}

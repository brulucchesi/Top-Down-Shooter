using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : Enemy
{
    private bool _canChase = false;

    protected override void FieldOfView()
    {
        base.FieldOfView();

        if (_currentDistance < _maxDistance && _currentAngle < _maxAngle && _currentAngle > -_maxAngle)
        {
            _canChase = true;
        }
        else
        {
            _canChase = false;
        }
    }

    protected override void Move()
    {
        base.Move();

        if (!_canChase)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
           
        Vector3 direction = _player.transform.position - transform.position;

        ShipObject.transform.up = Vector3.Lerp(ShipObject.transform.up, direction, Time.fixedDeltaTime * _rotateSpeed);

        _rigidbody.velocity = ShipObject.transform.up * _moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _canChase = false;

            GameObject o = collision.gameObject;
            Ship ship = o.transform.GetComponent<Ship>();

            if (ship != null)
            {
                ship.TakeDamage(DamageOther);
            }

            EnableShipCollider(false);

            _audioManager.PlayExplosionSFX();

            _animator.SetTrigger("Explode");
        }
    }
}

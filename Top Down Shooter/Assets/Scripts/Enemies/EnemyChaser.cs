using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : Enemy
{
    private bool _canChase = false;

    protected override void Update()
    {
        base.Update();

        ChasePlayer();
    }

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

    private void ChasePlayer()
    {
        if (!_canChase)
            return;

        Vector3 direction = _player.transform.position - this.transform.position;

        if (Mathf.Abs(_currentAngle) > 0.5f)
        {
            ShipObject.transform.Rotate(Vector3.forward * _currentAngle);
        }

        this.transform.Translate(direction.normalized * _moveSpeed * Time.deltaTime, Space.World);
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

            _animator.SetTrigger("Explode");
        }
    }
}

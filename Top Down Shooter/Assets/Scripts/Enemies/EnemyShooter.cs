using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipShooter))]
public class EnemyShooter : Enemy
{
    ShipShooter _shipShooterScript;

    [Header("Patrol")]
    [SerializeField]
    private float _patrolDistance = 10f;
    [SerializeField]
    private Transform _colliderCheck = null;

    [SerializeField]
    private float _distanceAccuracy = 0.5f;

    private bool _canPatrol = false;
    private bool _wasSeeingPlayer = false;

    private Vector3[] _patrolPoints = new Vector3[2];

    private Vector3 _pointToPatrol;
    private int _currentPointIndex;
    private Vector3 direction;

    private Coroutine _rotateShipRoutine;

    protected override void Start()
    {
        base.Start();
        _shipShooterScript = GetComponent<ShipShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _patrolPoints[0] = transform.position + ShipObject.transform.up * _patrolDistance;
        _patrolPoints[1] = transform.position - ShipObject.transform.up * _patrolDistance;

        _currentPointIndex = 0;
        _pointToPatrol = _patrolPoints[0];
        _canPatrol = true;
        _wasSeeingPlayer = false;

        direction = _pointToPatrol - transform.position;
    }

    protected override void Move()
    {
        base.Move();

        if (!_canPatrol)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        if (direction.magnitude > _distanceAccuracy && _cameraBounds.Contains(_colliderCheck.position))
        {
            _rigidbody.velocity = ShipObject.transform.up * _moveSpeed;
        }
        else
        {
            ChangePatrolPoint();
        }
    }

    private IEnumerator RotateShip()
    {
        direction = _pointToPatrol - transform.position;

        Quaternion initialRotation = ShipObject.transform.rotation;
        float fraction = 0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        Quaternion quaternion = Quaternion.AngleAxis(angle, ShipObject.transform.forward);

        while (fraction <= 1f)
        {
            fraction += Time.deltaTime * _rotateSpeed;

            ShipObject.transform.rotation = Quaternion.Lerp(initialRotation, quaternion, fraction);

            yield return new WaitForEndOfFrame();
        }

        _canPatrol = true;
        _rotateShipRoutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChangePatrolPoint();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ChangePatrolPoint();
    }

    private void ChangePatrolPoint()
    {
        if (!_canPatrol)
            return;
       
        _canPatrol = false;

        _rigidbody.velocity = Vector2.zero;
        _currentPointIndex++;

        if (_currentPointIndex > _patrolPoints.Length - 1)
            _currentPointIndex = 0;

        _pointToPatrol = _patrolPoints[_currentPointIndex];

        if (_rotateShipRoutine != null)
        {
            StopRotateRoutine();
        }

        _rotateShipRoutine = StartCoroutine(RotateShip());
    }

    private void StopRotateRoutine()
    {
        StopCoroutine(_rotateShipRoutine);

        _rotateShipRoutine = null;
    }

    public override void ResetShip()
    {
        _canPatrol = false;
        base.ResetShip();
    }

    protected override void FieldOfView()
    {
        base.FieldOfView();

        if (_currentDistance < _maxDistance && _currentAngle < _maxAngle && _currentAngle > -_maxAngle)
        {
            _canPatrol = false;
            _wasSeeingPlayer = true;

            if (_rotateShipRoutine != null)
            {
                StopRotateRoutine();
            }

            if (_currentAngle > 60f)
            {
                _shipShooterScript.SideShoot(true);
            }
            else if (_currentAngle < -60f)
            {
                _shipShooterScript.SideShoot();
            }
            else
            {
                direction = _player.transform.position - transform.position;
                ShipObject.transform.up = Vector3.Lerp(ShipObject.transform.up, direction, Time.deltaTime * _rotateSpeed);

                _shipShooterScript.FrontalShoot();
            }
        }
        else
        {
            if (_wasSeeingPlayer)
            {
                _rotateShipRoutine = StartCoroutine(RotateShip());
                _wasSeeingPlayer = false;
            }
        }

    }
}

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

    private void OnEnable()
    {
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

        Debug.DrawRay(transform.position, direction, Color.red);

        base.Move();
        if (!_canPatrol)
        {
            return;
        }

        //Debug.Log(_pointToPatrol);
        //ShipObject.transform.up = Vector3.Lerp(ShipObject.transform.up, direction, Time.deltaTime * _rotateSpeed);

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
        Quaternion initialRotation = ShipObject.transform.rotation;
        Quaternion finalRotation = Quaternion.FromToRotation(ShipObject.transform.up, direction);
        float fraction = 0f;

        while(fraction <= 1f)
        {
            fraction += Time.deltaTime * _rotateSpeed;

            Debug.Log("rotate " + fraction);

            ShipObject.transform.rotation = Quaternion.Lerp(initialRotation, finalRotation, fraction);

            yield return new WaitForEndOfFrame();
        }

        _canPatrol = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Change Patrol trigger");
        ChangePatrolPoint();
    }

    private void ChangePatrolPoint()
    {
        if (!_canPatrol)
            return;

        Debug.Log("Change Patrol direction");
        _canPatrol = false;

        _rigidbody.velocity = Vector2.zero;
        _currentPointIndex++;

        if (_currentPointIndex >= _patrolPoints.Length)
            _currentPointIndex = 0;

        _pointToPatrol = _patrolPoints[_currentPointIndex];
        direction = _pointToPatrol - transform.position;

        if (_rotateShipRoutine != null)
            StopCoroutine(_rotateShipRoutine);

        _rotateShipRoutine = StartCoroutine(RotateShip());
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
            Debug.Log("seeing player");

            _canPatrol = false;
            _wasSeeingPlayer = true;

            if (_rotateShipRoutine != null)
            {
                StopCoroutine(_rotateShipRoutine);

                Debug.Log(_rotateShipRoutine);
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
                Debug.Log("stop seeing player");

                _wasSeeingPlayer = false;
                _canPatrol = true;
            }
        }

    }
}

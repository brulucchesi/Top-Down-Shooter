using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    protected GameObject _player;

    [Header("Field Of View")]
    [SerializeField]
    protected float _maxDistance = 4f;

    [SerializeField]
    protected float _maxAngle = 100f;

    protected float _currentAngle;

    protected float _currentDistance;

    protected override void Start()
    {
        base.Start();

        _player = FindObjectOfType<Player>().gameObject;
    }

    protected override void Update()
    {
        base.Update();
        FieldOfView();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected virtual void FieldOfView()
    {
        _currentAngle = Vector3.SignedAngle(ShipObject.transform.up, _player.transform.position - transform.position, Vector3.forward);

        _currentDistance = Vector3.Distance(_player.transform.position, transform.position);
    }

    protected override void Death()
    {
        base.Death();
        Destroy(this.gameObject);
    }
}

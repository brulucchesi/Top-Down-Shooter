using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    protected Player _player;

    [SerializeField]
    private int _pointToPlayer = 1;

    [Header("Field Of View")]
    [SerializeField]
    protected float _maxDistance = 4f;

    [SerializeField]
    protected float _maxAngle = 100f;

    protected float _currentAngle;

    protected float _currentDistance;

    private EnemySpawnManager _enemySpawnManager;


    protected virtual void OnEnable()
    {
        EnableShipCollider(true);
    }

    protected override void Start()
    {
        base.Start();

        _player = _gameManager.Player;

        _enemySpawnManager = EnemySpawnManager.Instance;
    }

    protected override void FixedUpdate()
    {
        FieldOfView();
        base.FixedUpdate();
    }

    protected virtual void FieldOfView()
    {
        _currentAngle = Vector3.SignedAngle(ShipObject.transform.up, _player.transform.position - transform.position, Vector3.forward);

        _currentDistance = Vector3.Distance(_player.transform.position, transform.position);
    }

    protected override void Death()
    {
        base.Death();
        
        if(_currentHealth <= 0)
            _gameManager.AddPointsToPlayer(_pointToPlayer);

        ResetShip();
    }

    public override void ResetShip()
    {
        base.ResetShip();

        gameObject.SetActive(false);
        _enemySpawnManager.RemoveEnemy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ShipShooter))]
public class Player : Ship
{
    ShipShooter _shipShooterScript;

    private bool _canMove = false;

    [SerializeField]
    private float _timeToRevive = 1f;

    private ScreenManager _screenManager;

    protected override void Start()
    {
        base.Start();

        _gameManager.Player = this;
        _screenManager = ScreenManager.Instance;

        _shipShooterScript = GetComponent<ShipShooter>();
    }

    protected override void FixedUpdate()
    {
        if (!_canMove)
            return;

        base.FixedUpdate();
    }

    private void Update()
    {
        if (!_canMove)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _shipShooterScript.FrontalShoot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _shipShooterScript.SideShoot(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _shipShooterScript.SideShoot();
        }
    }

    protected override void Move()
    {
        base.Move();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _rigidbody.velocity = ShipObject.transform.up * Mathf.Max(verticalInput, 0) * _moveSpeed;

        float rotation = -horizontalInput * _rotateSpeed;

        ShipObject.transform.Rotate(Vector3.forward * rotation);
    }

    protected override void Death()
    {
        base.Death();

        ResetShip();

        StartCoroutine(ReviveRoutine());
    }

    public override void ResetShip()
    {
        base.ResetShip();

        _canMove = false;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private IEnumerator ReviveRoutine()
    {
        yield return new WaitForSeconds(_timeToRevive);

        if(_screenManager.GetCurrentScreen() != ScreenType.GameOver)
        {
            StartPlayer();
        }
    }

    public void StartPlayer()
    {
        Rect cameraBounds = _gameManager.GetCameraBounds();

        transform.position = cameraBounds.center;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        EnableShipCollider(true);
        _canMove = true;
    }
}

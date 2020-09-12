using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ShipShooter))]
public class Player : Ship
{
    ShipShooter _shipShooterScript;

    protected override void Start()
    {
        base.Start();
        _shipShooterScript = GetComponent<ShipShooter>();
    }

    protected override void Update()
    {
        base.Update();

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

        _rigidbody.velocity = ShipObject.transform.up * verticalInput * _moveSpeed;

        float rotation = -horizontalInput * _rotateSpeed;

        ShipObject.transform.Rotate(Vector3.forward * rotation);
    }
}

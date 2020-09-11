using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipShooter))]
public class EnemyShooter : Enemy
{
    ShipShooter _shipShooterScript;

    protected override void Start()
    {
        base.Start();
        _shipShooterScript = GetComponent<ShipShooter>();
    }

    protected override void FieldOfView()
    {
        base.FieldOfView();

        if (_currentDistance < _maxDistance && _currentAngle < _maxAngle && _currentAngle > -_maxAngle)
        {
            if (_currentAngle > 60f)
            {
                _shipShooterScript.SideShoot(true);
                Debug.Log("esquerda");
            }
            else if (_currentAngle < -60f)
            {
                _shipShooterScript.SideShoot();
                Debug.Log("direita");
            }
            else
            {
                if(Mathf.Abs(_currentAngle) > 0.5f)
                {
                    ShipObject.transform.Rotate(Vector3.forward * _currentAngle);
                }

                _shipShooterScript.FrontalShoot();
                Debug.Log("to vendo de frente");
            }
        }
    }
}

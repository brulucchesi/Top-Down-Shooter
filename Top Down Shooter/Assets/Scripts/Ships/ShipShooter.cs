using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class ShipShooter : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField]
    private GameObject _bulletPrefab = null;

    [SerializeField]
    private float _shootRateInSeconds = 1f;

    private float _canShoot = -1f;

    [SerializeField]
    private Transform _frontalShootSpawn = null;
    [SerializeField]
    private Transform _leftShootSpawns = null;
    [SerializeField]
    private Transform _rightShootSpawns = null;

    Ship _shipScript;

    private void Start()
    {
        _shipScript = GetComponent<Ship>();
    }

    public void FrontalShoot()
    {
        if (Time.time < _canShoot)
            return;

        _canShoot = Time.time + _shootRateInSeconds;

        CreateBullet(_frontalShootSpawn.position, _shipScript.ShipObject.transform.rotation);
    }

    public void SideShoot(bool isLeft = false)
    {
        if (Time.time < _canShoot)
            return;

        _canShoot = Time.time + _shootRateInSeconds;

        Transform spawns = _rightShootSpawns;

        if (isLeft)
        {
            spawns = _leftShootSpawns;
        }

        foreach (Transform child in spawns)
        {
            CreateBullet(child.position, child.rotation);
        }
    }

    private void CreateBullet(Vector3 pos, Quaternion rot)
    {
        GameObject o = Instantiate(_bulletPrefab, pos, rot);

        Bullet bullet = o.GetComponent<Bullet>();
        if (bullet != null)
        {
            if (gameObject.GetComponent<Player>() != null)
                bullet.AssignBulletToPlayer();

            bullet.AssignDamage(_shipScript.DamageOther);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class ShipShooter : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField]
    private float _shootRateInSeconds = 1f;

    private float _canShoot = -1f;

    [SerializeField]
    private Transform _frontalShootSpawn = null;
    [SerializeField]
    private Transform _leftShootSpawns = null;
    [SerializeField]
    private Transform _rightShootSpawns = null;

    private Ship _shipScript;

    private PoolManager _poolManager;

    private void Start()
    {
        _shipScript = GetComponent<Ship>();
        _poolManager = PoolManager.Instance;
    }

    public void FrontalShoot()
    {
        if (Time.time < _canShoot)
            return;

        _canShoot = Time.time + _shootRateInSeconds;

        GetBullet(_frontalShootSpawn.position, _shipScript.ShipObject.transform.rotation);
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
            GetBullet(child.position, child.rotation);
        }
    }

    private void GetBullet(Vector3 pos, Quaternion rot)
    {
        Transform bullet = _poolManager.GetBulletFromPool();

        bullet.position = pos;
        bullet.rotation = rot;

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            if (gameObject.GetComponent<Player>() != null)
                bulletScript.AssignBulletToPlayer();

            bulletScript.AssignDamage(_shipScript.DamageOther);
        }

        bullet.gameObject.SetActive(true);
    }
}

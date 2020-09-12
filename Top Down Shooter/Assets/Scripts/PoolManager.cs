using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region  Singleton
    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("PoolManager is NULL");

            return _instance;
        }
    }
    #endregion

    [Header("Pools")]
    [SerializeField]
    private Transform[] _enemiesPool = null;

    [SerializeField]
    private Transform _bulletsPool = null;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject[] _enemiesPrefabs = null;

    [SerializeField]
    private GameObject _bulletPrefab = null;

    [Header("Initial Quantity")]
    [SerializeField]
    private int _initialEnemiesPerType = 2;
    [SerializeField]
    private int _initialBullets = 5;

    private GameManager _gameManager;

    private void Awake()
    {
        _instance = this;

        for (int i = 0; i < _initialBullets; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity, _bulletsPool);
            bullet.SetActive(false);
        }

        for (int i = 0; i < _enemiesPrefabs.Length; i++)
        {
            for (int z = 0; z < _initialEnemiesPerType; z++)
            {
                GameObject enemy = Instantiate(_enemiesPrefabs[i], Vector3.zero, Quaternion.identity, _enemiesPool[i]);
                enemy.SetActive(false);
            }
        }
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public Transform GetBulletFromPool()
    {
        for (int i = 0; i < _bulletsPool.childCount; i++)
        {
            if (!_bulletsPool.GetChild(i).gameObject.activeInHierarchy)
            {
                return _bulletsPool.GetChild(i);
            }
        }

        Transform bullet = Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity, _bulletsPool).transform;

        bullet.gameObject.SetActive(false);

        return bullet;
    }

    public Transform GetEnemyFromPool(int index)
    {
        for (int i = 0; i < _enemiesPool[index].childCount; i++)
        {
            if (!_enemiesPool[index].GetChild(i).gameObject.activeInHierarchy)
            {
                return _enemiesPool[index].GetChild(i);
            }
        }

        Transform enemy = Instantiate(_enemiesPrefabs[index], Vector3.zero, Quaternion.identity, _enemiesPool[index]).transform;

        enemy.gameObject.SetActive(false);

        return enemy;
    }

    public int EnemiesPrefabsNumber()
    {
        return _enemiesPrefabs.Length;
    }
}

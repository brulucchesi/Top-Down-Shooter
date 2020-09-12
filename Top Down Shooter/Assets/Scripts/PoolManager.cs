using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum PoolType
{
    Bullet,
    Enemy
}

[Serializable]
public class PoolObject
{
    public PoolType Type;
    public Transform Parent;
    public GameObject Prefab;
}

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

    [SerializeField]
    private List<PoolObject> _pools = null;

    [Header("Initial Quantity")]
    [SerializeField]
    private int _initialEnemiesPerType = 2;
    [SerializeField]
    private int _initialBullets = 5;

    private GameManager _gameManager;

    private PoolObject[] _enemiesPools;

    private PoolObject _bulletPool;

    private void Awake()
    {
        _instance = this;

        _bulletPool = _pools.First(pool => pool.Type == PoolType.Bullet);

        for (int i = 0; i < _initialBullets; i++)
        {
            GameObject bullet = Instantiate(_bulletPool.Prefab, Vector3.zero, Quaternion.identity, _bulletPool.Parent);
            bullet.SetActive(false);
        }

        _enemiesPools = _pools.Where(pool => pool.Type == PoolType.Enemy).ToArray();

        for (int i = 0; i < _enemiesPools.Length; i++)
        {
            for (int z = 0; z < _initialEnemiesPerType; z++)
            {
                GameObject enemy = Instantiate(_enemiesPools[i].Prefab, Vector3.zero, Quaternion.identity, _enemiesPools[i].Parent);
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
        for (int i = 0; i < _bulletPool.Parent.childCount; i++)
        {
            if (!_bulletPool.Parent.GetChild(i).gameObject.activeInHierarchy)
            {
                return _bulletPool.Parent.GetChild(i);
            }
        }

        Transform bullet = Instantiate(_bulletPool.Prefab, Vector3.zero, Quaternion.identity, _bulletPool.Parent).transform;

        bullet.gameObject.SetActive(false);

        return bullet;
    }

    public Transform GetEnemyFromPool(int index)
    {
        for (int i = 0; i < _enemiesPools[index].Parent.childCount; i++)
        {
            if (!_enemiesPools[index].Parent.GetChild(i).gameObject.activeInHierarchy)
            {
                return _enemiesPools[index].Parent.GetChild(i);
            }
        }

        Transform enemy = Instantiate(_enemiesPools[index].Prefab, Vector3.zero, Quaternion.identity, _enemiesPools[index].Parent).transform;

        enemy.gameObject.SetActive(false);

        return enemy;
    }

    public int GetEnemyTypesCount()
    {
        return _enemiesPools.Length;
    }
}

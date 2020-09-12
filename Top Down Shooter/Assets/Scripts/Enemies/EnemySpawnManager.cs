using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    #region  Singleton
    private static EnemySpawnManager _instance;
    public static EnemySpawnManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("EnemySpawnManager is NULL");

            return _instance;
        }
    }
    #endregion

    [Header("Spawn Enemies")]
    [SerializeField]
    private float _secondsToSpawn = 1f;
    [SerializeField]
    private int _maxOfEnemiesOnScreen = 5;

    private bool _canSpawn = false;

    private PoolManager _poolManager;

    private GameManager _gameManager;

    private List<GameObject> _activeEnemies = new List<GameObject>();

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _poolManager = PoolManager.Instance;

        _gameManager = GameManager.Instance;

        _canSpawn = true;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (_canSpawn)
        {
            if (_activeEnemies.Count < _maxOfEnemiesOnScreen)
            {
                Transform enemy = _poolManager.GetEnemyFromPool(Random.Range(0, _poolManager.GetEnemyTypesCount()));

                if (enemy != null)
                {
                    Vector3 pos;

                    if (!GetRandomPosition(0, out pos))
                        continue;

                    enemy.position = pos;

                    GameObject ship = enemy.GetComponent<Ship>().ShipObject;
                    if(ship != null)
                    {
                        ship.transform.Rotate(Vector3.forward * Random.Range(0f, 360f));
                    }

                    enemy.gameObject.SetActive(true);

                    _activeEnemies.Add(enemy.gameObject);
                }
            }

            yield return new WaitForSeconds(_secondsToSpawn);
        }
    }

    private bool GetRandomPosition(int triesCount, out Vector3 pos)
    {
        pos = new Vector3(0, 0, 0);

        if (triesCount > 10)
        {
            return false;
        }

        Rect cameraBounds = _gameManager.GetCameraBounds();
        float randomX = Random.Range(cameraBounds.xMin, cameraBounds.xMax);
        float randomY = Random.Range(cameraBounds.yMin, cameraBounds.yMax);
        pos = new Vector3(randomX, randomY);

        Vector2 size = new Vector2(1f, 1f);

        Collider2D[] col = Physics2D.OverlapBoxAll(pos, size, 0);

        if (col != null && col.Length > 0)
        {
            triesCount++;
            return GetRandomPosition(triesCount, out pos);
        }

        return true;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _activeEnemies.Remove(enemy);
    }
}

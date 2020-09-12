using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region  Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("GameManager is NULL");

            return _instance;
        }
    }
    #endregion

    private PoolManager _poolManager;

    [Header("Spawn Enemies")]
    [SerializeField]
    private float _minSecondsToSpawn = 1f;
    [SerializeField]
    private float _maxSecondsToSpawn = 5f;

    private bool _canSpawn = false;

    private Rect _cameraBounds;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _poolManager = PoolManager.Instance;
        _canSpawn = true;
        StartCoroutine(SpawnEnemies());
    }

    public Rect GetCameraBounds()
    {
        Vector3 cameraStart = Camera.main.ScreenToWorldPoint(Vector3.zero);

        Vector3 cameraSizeInPixels = new Vector3(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight, 0);
        Vector3 cameraEnd = Camera.main.ScreenToWorldPoint(cameraSizeInPixels);

        float x = cameraStart.x;
        float y = cameraStart.y;
        float width = cameraEnd.x - cameraStart.x;
        float height = cameraEnd.y - cameraStart.y;

        _cameraBounds = new Rect(x, y, width, height);

        return _cameraBounds;
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForEndOfFrame();
        while (_canSpawn)
        {
            Transform enemy = _poolManager.GetEnemyFromPool(Random.Range(0, _poolManager.EnemiesPrefabsNumber()));
            Debug.Log("spawn");
            if (enemy != null)
            {
                Vector3 pos;

                if (!GetRandomPosition(0, out pos))
                    continue;

                enemy.position = pos;
                enemy.gameObject.SetActive(true);
                //enemy.gameObject.GetComponent<EnemyScript>().CallEnemyToAction();
            }

            yield return new WaitForSeconds(Random.Range(_minSecondsToSpawn, _maxSecondsToSpawn));
        }
    }

    private bool GetRandomPosition(int triesCount, out Vector3 pos)
    {
        pos = new Vector3(0, 0, 0);

        if (triesCount > 10)
        {
            return false;
        }

        float randomX = Random.Range(_cameraBounds.xMin, _cameraBounds.xMax);
        float randomY = Random.Range(_cameraBounds.yMin, _cameraBounds.yMax);
        pos = new Vector3(randomX, randomY);

        //RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.up, 2f);

        //if(hit.collider != null)
        //{
        //    Debug.Log("alo");
        //    return GetRandomPosition();
        //}

        Vector2 size = new Vector2(1f, 1f);

        Collider2D[] col = Physics2D.OverlapBoxAll(pos, size, 0);

        //Collider[] col = Physics.OverlapBox(pos, extents);
        Debug.Log(col.Length);

        if (col != null && col.Length > 0)
        {
            Debug.Log("alo");
            triesCount++;
            return GetRandomPosition(triesCount, out pos);
        }

        //if (Physics.Raycast(pos, Vector3.forward,out hit, Mathf.Infinity))
        //{
        //    Debug.Log("hit " + hit);
        //}

        return true;
    }
}

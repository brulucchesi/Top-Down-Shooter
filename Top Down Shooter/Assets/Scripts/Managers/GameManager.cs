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

    private Rect _cameraBounds;

    [SerializeField]
    private GameObject _playerPrefab = null;

    [HideInInspector]
    public Player Player;

    private EnemySpawnManager _enemySpawnManager;

    private ScreenManager _screenManager;

    [HideInInspector]
    public int PlayerPoints = 0;

    public delegate void OnChangePlayerPoint();
    public static event OnChangePlayerPoint onChangePlayerPoint;

    public delegate void OnChangeTime();
    public static event OnChangeTime onChangeTime;

    [HideInInspector]
    public float TimeToEnd = 0f;
    private bool _canCountTime = false;
    
    [SerializeField]
    private float _defaultSessionTimeInSeconds = 60f;

    private void Awake()
    {
        _instance = this;

        Player = FindObjectOfType<Player>();

        if(Player == null)
            Player = Instantiate(_playerPrefab).GetComponent<Player>();

        Player.ResetShip();
    }

    private void Start()
    {
        _enemySpawnManager = EnemySpawnManager.Instance;
        _screenManager = ScreenManager.Instance;
    }

    public void StartGame()
    {
        Player.StartPlayer();
        _enemySpawnManager.StartSpawn();
        PlayerPoints = 0;

        TimeToEnd = PlayerPrefs.GetFloat("SessionTime", _defaultSessionTimeInSeconds);
        _canCountTime = true;
    }

    public float GetDefaultSessionTime()
    {
        return _defaultSessionTimeInSeconds;
    }

    private void GameOver()
    {
        Player.ResetShip();

        _enemySpawnManager.StopSpawn();

        _screenManager.ChangeScreen(ScreenType.GameOver);
    }

    private void Update()
    {
        ChangeTime();
    }

    private void ChangeTime()
    {
        if (_canCountTime)
        {
            if (TimeToEnd > 0)
            {
                if (onChangeTime != null)
                    onChangeTime();

                TimeToEnd -= Time.deltaTime;
            }
            else
            {
                TimeToEnd = 0f;
                _canCountTime = false;
                GameOver();
            }
        }
    }

    public void AddPointsToPlayer(int point)
    {
        PlayerPoints += point;

        if (onChangePlayerPoint != null)
            onChangePlayerPoint();
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
}

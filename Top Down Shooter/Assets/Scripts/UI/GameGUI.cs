using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameGUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _pointsText = null;

    [SerializeField]
    private TextMeshProUGUI _timeText = null;

    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;

        _gameManager.StartGame();

        GameManager.onChangePlayerPoint += ChangePoints;
        GameManager.onChangeTime += ChangeTime;

        ChangePoints();
    }

    private void OnDisable()
    {
        GameManager.onChangePlayerPoint -= ChangePoints;
        GameManager.onChangeTime -= ChangeTime;
    }

    public void ChangePoints()
    {
        _pointsText.text = _gameManager.PlayerPoints.ToString();
    }

    public void ChangeTime()
    {
        int minutes = Mathf.FloorToInt(_gameManager.TimeToEnd / 60);
        int seconds = Mathf.FloorToInt(_gameManager.TimeToEnd % 60);

        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

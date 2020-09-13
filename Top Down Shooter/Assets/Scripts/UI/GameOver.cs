using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _pointsText = null;

    [SerializeField]
    private Button _playButton = null;

    [SerializeField]
    private Button _menuButton = null;

    private ScreenManager _screenManager;
    private GameManager _gameManager;

    void Start()
    {
        _screenManager = ScreenManager.Instance;

        _playButton.onClick.AddListener(() => _screenManager.ChangeScreen(ScreenType.GameGUI));
        _menuButton.onClick.AddListener(() => _screenManager.ChangeScreen(ScreenType.MainMenu));
    }

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;

        _pointsText.text = _gameManager.PlayerPoints.ToString();
    }
}

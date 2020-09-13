using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button _playButton = null;
    [SerializeField]
    private Button _options = null;

    private ScreenManager _screenManager;
    
    void Start()
    {
        _screenManager = ScreenManager.Instance;

        _playButton.onClick.AddListener(() => _screenManager.ChangeScreen(ScreenType.GameGUI));
        _options.onClick.AddListener(() => _screenManager.ChangeScreen(ScreenType.Options));
    }
}

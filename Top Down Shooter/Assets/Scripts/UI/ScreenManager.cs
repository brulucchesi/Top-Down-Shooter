using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ScreenType
{
    MainMenu,
    Options,
    GameOver,
    GameGUI
}

[Serializable]
public class Screen
{
    public ScreenType Type;
    public GameObject ScreenObject;
}

public class ScreenManager : MonoBehaviour
{
    #region  Singleton
    private static ScreenManager _instance;
    public static ScreenManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("ScreenManager is NULL");

            return _instance;
        }
    }
    #endregion

    [SerializeField]
    private List<Screen> _screens = null;

    void Awake()
    {
        _instance = this;
        ChangeScreen(ScreenType.MainMenu);
    }

    public void ChangeScreen(ScreenType screenType)
    {
        foreach (Screen screen in _screens)
        {
            screen.ScreenObject.SetActive(false);
        }

        _screens.First(screen => screen.Type == screenType).ScreenObject.SetActive(true);
    }
}

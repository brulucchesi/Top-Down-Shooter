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

    public Player Player;

    private void Awake()
    {
        _instance = this;
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

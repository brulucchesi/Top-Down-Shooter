using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField]
    private Slider _gameSessionSlider = null;

    [SerializeField]
    private TMP_InputField _enemySpawnInput = null;

    [SerializeField]
    private Button _backButton = null;

    private ScreenManager _screenManager;
    private GameManager _gameManager;
    private EnemySpawnManager _enemySpawnManager;

    private void Start()
    {
        _screenManager = ScreenManager.Instance;

        _gameSessionSlider.onValueChanged.AddListener(ChangeGameSession);

        _enemySpawnInput.onValueChanged.AddListener(ChangeEnemySpawn);

        _backButton.onClick.AddListener(() => _screenManager.ChangeScreen(ScreenType.MainMenu));
    }

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _enemySpawnManager = EnemySpawnManager.Instance;

        float sessionTime = PlayerPrefs.GetFloat("SessionTime", _gameManager.GetDefaultSessionTime());
        _gameSessionSlider.value = sessionTime;
        UpdateSessionHandle(sessionTime);

        float enemySpawnTime = PlayerPrefs.GetFloat("EnemySpawnTime", _enemySpawnManager.GetDefaultSecondsToSpawn());
        _enemySpawnInput.placeholder.GetComponent<TextMeshProUGUI>().text = enemySpawnTime.ToString();
    }

    private void ChangeGameSession(float time)
    {
        UpdateSessionHandle(time);
        PlayerPrefs.SetFloat("SessionTime", time);
    }

    private void UpdateSessionHandle(float time)
    {
        string handleText = string.Format("{0:0.0}", time / 60);
        _gameSessionSlider.handleRect.GetComponent<TextMeshProUGUI>().text = handleText + " min";
    }

    private void ChangeEnemySpawn(string time)
    {
        float finalValue;

        if (!float.TryParse(time, out finalValue))
        {
            finalValue = PlayerPrefs.GetFloat("EnemySpawnTime", _enemySpawnManager.GetDefaultSecondsToSpawn());
            _enemySpawnInput.placeholder.GetComponent<TextMeshProUGUI>().text = finalValue.ToString();
        }
        else
        {
            PlayerPrefs.SetFloat("EnemySpawnTime", finalValue);
        }
    }
}

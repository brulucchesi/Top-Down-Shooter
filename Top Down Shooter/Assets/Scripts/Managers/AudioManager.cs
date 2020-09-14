using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region  Singleton
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("AudioManager is NULL");

            return _instance;
        }
    }
    #endregion
    [SerializeField]
    private AudioSource _sfxAudioSource = null;

    [SerializeField]
    private AudioClip _explosionSFX = null;

    private void Awake()
    {
        _instance = this;
    }

    public void PlayExplosionSFX()
    {
        _sfxAudioSource.PlayOneShot(_explosionSFX, 0.1f);
    }
}

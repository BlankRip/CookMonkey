using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME_KEY, 0.3f);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private const string PLAYER_PREFS_MUSIC_VOLUME_KEY = "MusicVolume";
    private float volume = 0.3f;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1.05f)
        {
            volume = 0.0f;
        }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME_KEY, volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}

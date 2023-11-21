using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source")]
    public AudioSource musicSource, sfxSource;

    [Header("Audio Clip")] public AudioClip startGame,background, player, box, push;

    private void Awake()
    {
        if(Instance != null) Debug.LogWarning("Onlly 1 AudioManager");
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LoadMusicGame();
    }

    public void PlaySFX(AudioClip _clip)
    {
        sfxSource.PlayOneShot(_clip);
    }

    void LoadMusicGame()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            musicSource.clip = background;
            musicSource.Play();
        }
        else
        {
            musicSource.clip = startGame;
            musicSource.Play();
        }
    }
}

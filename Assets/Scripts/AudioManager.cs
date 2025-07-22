using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        audioSource = Camera.main!.gameObject.GetComponent<AudioSource>();
    }
    
    private AudioSource buttoAudioSource;
    public AudioClip buttonAudioClip;
    
    public AudioSource audioSource;

    void Start()
    {
        buttoAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayButtonClickAudio()
    {
        buttoAudioSource.PlayOneShot(buttonAudioClip, 0.1f);
    }

    public void SetAudioState(GameState state)
    {
        if (state == GameState.Pause)
        {
            audioSource.Pause();
        }
        else if (state == GameState.Game)
        {
            audioSource.UnPause();
        }
        else if (state == GameState.GameOver)
        {
            audioSource.Stop();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOfObject : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip [] sound;
    private bool sound_settings;

    public static SoundOfObject Instance { get; set; } //Для сбора и отправки данных из этого скрипта
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        sound_settings = SaveSerial.Instance.sound;
        if (!sound_settings) _audioSource.volume = 0;
        if (sound_settings) _audioSource.volume = 1;
    }
    public void PlaySound()
    {
        _audioSource.clip = sound[0];
        if (!_audioSource.isPlaying) _audioSource.Play();
    }
    public void ContinueSound()
    {
        _audioSource.clip = sound[0];
        if (!_audioSource.isPlaying) _audioSource.Play();
    }
    public void StopSound()
    {
        _audioSource.Stop();
    }
}

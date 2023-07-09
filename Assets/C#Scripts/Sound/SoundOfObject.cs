using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOfObject : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip [] sound;
    private bool sound_settings;
    private float volume;

    public static SoundOfObject Instance { get; set; } //to collect and send data from this script
    void Start()
    {
        _audioSource = this.gameObject.GetComponent<AudioSource>();
        Instance = this;
        volume = _audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        sound_settings = SaveSerial.Instance.sound;
        if (!sound_settings) _audioSource.volume = 0;
        if (sound_settings) _audioSource.volume = volume;
    }
    public void PlaySound()
    {
        _audioSource.clip = sound[0];
        _audioSource.Play();
    }
    public void ContinueSound()
    {
        _audioSource.clip = sound[0];
        if (!_audioSource.isPlaying) _audioSource.Play();
    }
    public void StopSound()
    {
        if (_audioSource.isPlaying) _audioSource.Stop();
    }
}

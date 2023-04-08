using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip [] songs;
    private bool music_settings;
    private float volume;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (!_audioSource.isPlaying) ChangeSong(Random.Range(0, songs.Length));
        volume = _audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        music_settings = SaveSerial.Instance.music;
        if (!music_settings) _audioSource.volume = 0;
        if (music_settings) _audioSource.volume = volume;

        if (!_audioSource.isPlaying) ChangeSong(Random.Range(0, songs.Length));
    }
    public void ChangeSong(int songPicked)
    {
        _audioSource.clip = songs[songPicked];
        _audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer _audioMixer;
    public AudioMixerGroup _musicGroup, _sfxGroup;
    public static AudioManager Instance;
    public bool _musicMuted, _sfxMuted;
    public AudioSource _musicSource;
    public AudioSource _soundSource;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //_musicSource = gameObject.GetComponentInChildren<AudioSource>();
    }
    private void Start()
    {
        if (_sfxMuted == true)
        {
            _audioMixer.SetFloat("SFXVol", -80.0f);
        }
        else
        {
            _audioMixer.SetFloat("SFXVol", 0);
        }

        if (_musicMuted == true)
        {
            _audioMixer.SetFloat("MusicVol", -80.0f);
        }
        else
        {
            _audioMixer.SetFloat("MusicVol", 0);
        }
    }
    public void ChangeMusic(AudioClip clip)
    {
        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.Play();

    }

    public void PlayASound(AudioClip sound, bool randomizePitch, bool stopPrevious)
    {
        if (stopPrevious)
        {
            _soundSource.Stop();
        }
        if (randomizePitch) 
        {
            float pitch = Random.Range(0.85f, 1.15f);
            _soundSource.pitch = pitch;
        }else
        {
            _soundSource.pitch = 1;
        }
        _soundSource.PlayOneShot(sound);
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void MusicVolume(float volume)
    {
        if (_musicMuted == false)
        {
            _audioMixer.SetFloat("MusicVol", volume);
        }
    }
    
}

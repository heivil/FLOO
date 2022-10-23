using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWork : MonoBehaviour
{
    public ParticleSystemRenderer _particleRend, _subRend;
    //public ParticleSystem _particle;
    private ParticleSystem.MainModule _particleMain;
    public Color _orange, _blue, _green, _purple;
    public Material[] _materials = new Material[3];
    private AudioSource _audioSource;
    public AudioClip[] _sounds = new AudioClip[3];

    private void Awake()
    {
        //_particleMain = _particle.main;
        //_subRend = _particleRend.gameObject.GetComponentInChildren<ParticleSystemRenderer>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        int random = Random.Range(0, _sounds.Length);
        _audioSource.clip = _sounds[random];
    }
    private void OnEnable()
    {
        _audioSource.Play();
        int random = Random.Range(0,4);
        if (random == 0) 
        {
            //_particleMain.startColor = _orange;
            _particleRend.material = _materials[0];
            _subRend.material = _materials[0];
            //_particleRend.trailMaterial = _materials[0
        }
        else if(random == 1)
        {
            //_particleMain.startColor = _green;
            _particleRend.material = _materials[1];
            _subRend.material = _materials[1];
            //_particleRend.trailMaterial = _materials[1];
        }
        else if(random == 2)
        {
            //_particleMain.startColor = _blue;
            _particleRend.material = _materials[2];
            _subRend.material = _materials[2];
            //_particleRend.trailMaterial = _materials[2];
        }else
        {
            _particleRend.material = _materials[3];
            _subRend.material = _materials[3];
        }
    }
}

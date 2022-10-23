using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Key : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    private Timer _timer;
    [HideInInspector]
    public int _idNumber;
    public GameObject _particle;
    private AudioSource _audioSource;
    private void Awake()
    {
        transform.position = GameManager.Instance.LevelManager._keySpawnPos.position;
        _idNumber = GameManager.Instance.LevelManager._keyID;
        if (GameManager.Instance.CheckKey(_idNumber) == true)
        {
            Destroy(gameObject);
        }
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _timer = gameObject.AddComponent<Timer>();
        _timer.OnTimerCompleted += DoneFeedbacking;
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (_timer != null) {
            _timer.OnTimerCompleted -= DoneFeedbacking;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {  
            _collider.enabled = false;
            GameManager.Instance.LevelManager._keyCollected = true;
            _renderer.enabled = false;
            _particle.gameObject.SetActive(true);
            _audioSource.Play();
            _timer.StartTimer(1);
        }
    }
    private void DoneFeedbacking() 
    {
        gameObject.SetActive(false);
    }
}

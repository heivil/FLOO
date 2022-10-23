using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumbleBee : MonoBehaviour
{
    public Transform _target1, _target2;
    private int _moveInt = 0;
    public float _speed = 1;
    private SpriteRenderer _renderer;
    public AudioClip _angryNoise;
    public AudioSource _audioSource;
    public GameObject _burn;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Vector2.Distance(_target1.position, transform.position) < 0.1f && _moveInt == 0)
        {
            _moveInt = 1;
            _renderer.flipX = true;

        } else if (Vector2.Distance(_target2.position, transform.position) < 0.1f && _moveInt == 1)
        {
            _moveInt = 0;
            _renderer.flipX = false;
        }

        if (_moveInt == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target1.position, Time.deltaTime * _speed); 
        }
        else if (_moveInt == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target2.position, Time.deltaTime * _speed);
        }
    }

    /// <summary>
    /// If the bumblebee is hit with flames collider, it dies and my girlfriend will get angry 
    /// </summary>
    /// <param name="collision">Other collider</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Flame>() != null)
        {
            GameManager.Instance._bumbleBee = true;
            _audioSource.PlayOneShot(_angryNoise);
            _audioSource.gameObject.transform.parent = null;
            _burn.SetActive(true);
            _burn.gameObject.transform.parent = null;
            gameObject.SetActive(false);
        }
    }
}

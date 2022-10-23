using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Timer _timer;
    private RepeatingTimer _flashTimer;
    public float _timeUntilBoom = 2;
    public GameObject _explosion;
    public bool _ignoreBadBlocks = false;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _timer = gameObject.AddComponent<Timer>();
        _flashTimer = gameObject.AddComponent<RepeatingTimer>();
        _timer.OnTimerCompleted += Explode;
        _flashTimer.OnTimerCompleted += Flash;
        
    }
    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= Explode;
        _flashTimer.OnTimerCompleted -= Flash;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_timer._isRunning == false && collision.gameObject.layer == 8 || _timer._isRunning == false && collision.gameObject.layer == 9 && _ignoreBadBlocks == false)
        {
            _timer.StartTimer(_timeUntilBoom);
            _renderer.color = Color.red;
            _flashTimer.StartTimer(0.25f, 8);
        }
    }

    /// <summary>
    /// starts flashing when about to explode
    /// </summary>
    private void Flash()
    {
        if (_renderer.color == Color.white)
        {
            _renderer.color = Color.red;
        }else if (_renderer.color == Color.red)
        {
            _renderer.color = Color.white;
        }
    }

    public void Explode()
    {
        _explosion.transform.parent = null;
        _explosion.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8 )
        {
            if (collision.gameObject.GetComponent < Goo> () != null)
            {
                collision.gameObject.GetComponent<Goo>()._jumpAmount = collision.gameObject.GetComponent<Goo>()._maxJumpAmount;
            }else if (collision.gameObject.GetComponent<Flame>() != null )
            {
                Explode();
            }
        }else if (collision.gameObject.CompareTag("CannonBall"))
        {
            if (collision.relativeVelocity.magnitude > 10)
            {
                Explode();
            }else if(_timer._isRunning == false)
            {
                _timer.StartTimer(_timeUntilBoom);
                _renderer.color = Color.red;
                _flashTimer.StartTimer(0.25f, 8);
            }
        }
    }
}

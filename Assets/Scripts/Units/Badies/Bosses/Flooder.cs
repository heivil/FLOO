using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flooder : BadGuyUnitBase
{
    public BoxCollider2D _floodCollider;
    private Timer _floodTimer, _intervalTimer;
    private Vector2 _maxFlood = new Vector2(14.0f, 16.7f), _noFlood;
    public float _floodRiseTime = 8, _floodLowerTime = 6, _downIntervalTime = 5, _upIntervalTime = 2;
    private bool _flood = false;
    public SpriteRenderer _floodRenderer;
    public Collider2D _dontTouchMeCollider;

    protected override void Awake()
    {
        base.Awake();
        _noFlood = _floodCollider.size;
        _floodTimer = gameObject.AddComponent<Timer>();
        _intervalTimer = gameObject.AddComponent<Timer>();
        //_intervalTimer.StartTimer(_intervalTime);
        _intervalTimer.OnTimerCompleted += FloodControl;
        _floodTimer.OnTimerCompleted += StartInterval;
        _hitTimer.OnTimerCompleted += FloodControl;
        FloodControl();
    }

    private void OnDestroy()
    {
        _intervalTimer.OnTimerCompleted -= FloodControl;
        _floodTimer.OnTimerCompleted -= StartInterval;
        _hitTimer.OnTimerCompleted -= FloodControl;
    }

    /// <summary>
    /// Makes flood that hurts player go up from floor or down towards floor, according to timers normalized time elapsed
    /// </summary>
    private void Update()
    {
        if (_flood && _floodTimer._isRunning)
        {
            _floodCollider.size = Vector2.Lerp(_noFlood, _maxFlood, _floodTimer.NormalizedTimeElapsed);
            _floodCollider.offset = new Vector2(0, _floodCollider.size.y / 2);
            if (_floodCollider.size.y > 0.5f)
            {
                _floodRenderer.size = new Vector2(16, _floodCollider.size.y + 0.5f);
            }
            else if(_floodCollider.size.y > 0.1f)
            {
                _floodRenderer.size = new Vector2(16, _floodCollider.size.y + _floodCollider.size.y);
            }

            _dontTouchMeCollider.enabled = true;
        }
        else if (_flood == false && _floodTimer._isRunning)
        {
            _floodCollider.size = Vector2.Lerp(_maxFlood, _noFlood, _floodTimer.NormalizedTimeElapsed);
            _floodCollider.offset = new Vector2(0, _floodCollider.size.y / 2);
            if (_floodCollider.size.y > 0.5f) 
            {
                _floodRenderer.size = new Vector2(16, _floodCollider.size.y + 0.5f);
            }
            else if (_floodCollider.size.y > 0.1f)
            {
                _floodRenderer.size = new Vector2(16, _floodCollider.size.y + _floodCollider.size.y);
            }

            _dontTouchMeCollider.enabled = false;
        }
        else
        {
            _dontTouchMeCollider.enabled = false;
        }
        
    }

    /// <summary>
    /// Determines enemy behaviour between flood goin up or down
    /// </summary>
    private void StartInterval()
    {
        if(_intervalTimer._isRunning == false && _hitTimer._isRunning == false)
        {
            if (_flood)
            {
                _intervalTimer.StartTimer(_upIntervalTime);
            }else
            {
                _intervalTimer.StartTimer(_downIntervalTime);
                _animator.SetBool("Tired", true);
            }
        }
        
    }

    /// <summary>
    /// After timer completes, changes mode of flood and changes animation accordingly 
    /// </summary>
    private void FloodControl()
    {
        _animator.SetBool("Tired", false);
        if (_flood)
        {
            _animator.SetBool("Flood", false);
            UnFlood();
        }
        else
        {
            _animator.SetBool("Flood", true);
            Flood();
        }
    }

    /// <summary>
    /// Determines that flood goes up and starts timer
    /// </summary>
    private void Flood()
    {
        _flood = true;
        if(_floodTimer._isRunning == false)
        {
            _floodTimer.StartTimer(_floodRiseTime);
        }
    }

    /// <summary>
    /// Determines that flood goes down and starts timer
    /// </summary>
    private void UnFlood()
    {
        _flood = false;
        if (_floodTimer._isRunning == false)
        {
            _floodTimer.StartTimer(_floodLowerTime);
        }
    }

    /// <summary>
    /// Takes damage when hit by player flame
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && _animator.GetBool("Tired") == true)
        {
            if (collision.gameObject.GetComponent<Flame>() != null)
            {
                _hitPoints--;
                if (_hitPoints <= 0)
                {
                    Die();
                }
                else
                {
                    _intervalTimer.StopTimer();
                    _animator.SetBool("Tired", false);
                    _animator.SetTrigger("Hit");
                    if (_hitSound != null) _audiosource.PlayOneShot(_hitSound);
                    if (_hitTimer._isRunning == false) 
                    { 
                        _hitTimer.StartTimer(_hitTime); 
                    }

                    collision.gameObject.GetComponent<Flame>().BounceFormEnemy(transform);
                }
            }
        }
    }
}

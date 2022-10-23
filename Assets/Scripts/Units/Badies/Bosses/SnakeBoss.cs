using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoss : BadGuyUnitBase
{
    private Timer _revealTimer, _chargeTimer, _cancelTimer, _waitTimer;
    public float _chargeTime = 1, _revealTime = 3, _waitTime = 1.5f;
    public List<Transform> _topTransGroup, _midTransGroup, _bottomTransGroup, _activeTransGroup;
    private bool _canCharge = false;
    private Vector2 _startPos, _endPos;

    private void OnDestroy()
    {
        _revealTimer.OnTimerCompleted -= DoneRevealing;
        _chargeTimer.OnTimerCompleted -= DoneCharging;
        _cancelTimer.OnTimerCompleted -= DoneCanceling;
        _waitTimer.OnTimerCompleted -= DoneWaiting;
    }
    protected override void Awake()
    {
        base.Awake();
        _animator = gameObject.GetComponent<Animator>();
        _revealTimer = gameObject.AddComponent<Timer>();
        _revealTimer.OnTimerCompleted += DoneRevealing;
        _chargeTimer = gameObject.AddComponent<Timer>();
        _chargeTimer.OnTimerCompleted += DoneCharging;
        _cancelTimer = gameObject.AddComponent<Timer>();
        _cancelTimer.OnTimerCompleted += DoneCanceling;
        _waitTimer = gameObject.AddComponent<Timer>();
        _waitTimer.OnTimerCompleted += DoneWaiting;
        transform.position = _topTransGroup[4].transform.position;
    }

    private void Update()
    {
        if (_revealTimer._isRunning)
        {
            transform.position = Vector2.Lerp(_startPos, _endPos, _revealTimer.NormalizedTimeElapsed);
        }else if (_cancelTimer._isRunning)
        {
            transform.position = Vector2.Lerp(_startPos, _endPos, _cancelTimer.NormalizedTimeElapsed);
        }else if (_chargeTimer._isRunning)
        {
            transform.position = Vector2.Lerp(_startPos, _endPos, _chargeTimer.NormalizedTimeElapsed);
        }
    }

    /// <summary>
    /// Moves enemy unit to the same level player is on so it can charge at player
    /// </summary>
    /// <param name="player">Player unit that triggers charging</param>
    /// <param name="targeter">Area of the trigger that determines whether to target player or not</param>
    public void Target(GameObject player, Transform targeter)
    {
        if (_revealTimer._isRunning == false && _canCharge == false && _cancelTimer._isRunning == false && _waitTimer._isRunning == false)
        {
            if (targeter.position.y > 4)
            {
                _activeTransGroup = _topTransGroup;
                if (transform.position.x > 1)
                {
                    _startPos = _topTransGroup[4].transform.position;
                    _endPos = _topTransGroup[3].transform.position;
                }
                else if (transform.position.x < -1)
                {
                    _startPos = _topTransGroup[5].transform.position;
                    _endPos = _topTransGroup[1].transform.position;
                }
            }
            else if (targeter.position.y < 1 && targeter.position.y > -1)
            {
                _activeTransGroup = _midTransGroup;
                if (transform.position.x > 1)
                {
                    _startPos = _midTransGroup[4].transform.position;
                    _endPos = _midTransGroup[3].transform.position;
                }
                else if (transform.position.x < -1)
                {
                    _startPos = _midTransGroup[5].transform.position;
                    _endPos = _midTransGroup[1].transform.position;
                }
            }
            else if (targeter.position.y < -4)
            {
                _activeTransGroup = _bottomTransGroup;
                if (transform.position.x > 1)
                {
                    _startPos = _bottomTransGroup[4].transform.position;
                    _endPos = _bottomTransGroup[3].transform.position;
                }
                else if (transform.position.x < -1)
                {
                    _startPos = _bottomTransGroup[5].transform.position;
                    _endPos = _bottomTransGroup[1].transform.position;
                }
            }
            _revealTimer.StartTimer(_revealTime);
        }
    }

    /// <summary>
    /// WHen player moves from targeting area, clears target and hides
    /// </summary>
    public void ClearTarget()
    {
        if (_revealTimer._isRunning && GameManager.Instance.LevelManager._player._snakeDoNotSwitchTargetLol == false)
        {
            float time = _revealTimer.CurrentTime;
            _revealTimer.StopTimer();
            if (_activeTransGroup == _topTransGroup)
            {
                if (transform.position.x > 1)
                {
                    _startPos = transform.position;
                    _endPos = _topTransGroup[4].transform.position;
                }
                else if (transform.position.x < -1)
                {
                    _startPos = transform.position;
                    _endPos = _topTransGroup[5].transform.position;
                }
            }
            else if (_activeTransGroup == _midTransGroup)
            {
                if (transform.position.x > 1)
                {
                    _startPos = transform.position;
                    _endPos = _midTransGroup[4].transform.position;
                }
                else if (transform.position.x < -1)
                {
                    _startPos = transform.position;
                    _endPos = _midTransGroup[5].transform.position;
                }
            }
            else if (_activeTransGroup == _bottomTransGroup)
            {
                if (transform.position.x > 1)
                {
                    _startPos = transform.position;
                    _endPos = _bottomTransGroup[4].transform.position;
                }
                else if (transform.position.x < -1)
                {
                    _startPos = transform.position;
                    _endPos = _bottomTransGroup[5].transform.position;
                }
            }
            _cancelTimer.StartTimer(time);
            _activeTransGroup = null;
        }
    }

    /// <summary>
    /// Slowly reveals enemy unit from dark smoke
    /// </summary>
    private void DoneRevealing()
    {
        if(_activeTransGroup == _topTransGroup)
        {
            if (transform.position.x > 1)
            {
                _startPos = _topTransGroup[3].transform.position;
                _endPos = _topTransGroup[0].transform.position;
            }
            else if (transform.position.x < -1)
            {
                _startPos = _topTransGroup[1].transform.position;
                _endPos = _topTransGroup[2].transform.position;
            }
        }
        else if (_activeTransGroup == _midTransGroup)
        {
            if (transform.position.x > 1)
            {
                _startPos = _midTransGroup[3].transform.position;
                _endPos = _midTransGroup[0].transform.position;
            }
            else if (transform.position.x < -1)
            {
                _startPos = _midTransGroup[1].transform.position;
                _endPos = _midTransGroup[2].transform.position;
            }
        }
        else if (_activeTransGroup == _bottomTransGroup)
        {
            if (transform.position.x > 1)
            {
                _startPos = _bottomTransGroup[3].transform.position;
                _endPos = _bottomTransGroup[0].transform.position;
            }
            else if (transform.position.x < -1)
            {
                _startPos = _bottomTransGroup[1].transform.position;
                _endPos = _bottomTransGroup[2].transform.position;
            }
        }
        _waitTimer.StartTimer(_waitTime);
        _canCharge = true;
    }

    /// <summary>
    /// Triggers small animation to warn player of coming attack
    /// </summary>
    private void DoneWaiting()
    {
        _animator.SetTrigger("GetReady");
    }

    /// <summary>
    /// Starts charge and the timer that determines charge speed and length
    /// </summary>
    private void Ready()
    {
        _animator.SetBool("Charge", true);
        _chargeTimer.StartTimer(_chargeTime);
    }

    /// <summary>
    /// Flips unit to face correct direction after chargin across screen and changes animation and prevents charging again until the appropriate time
    /// </summary>
    private void DoneCharging()
    {
        if(transform.position.x < -1)
        {
            transform.localScale = new Vector3(-1,1,1);
        }else if (transform.position.x > 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        _canCharge = false;
        _animator.SetBool("Charge", false);
    }

    /// <summary>
    /// if charge is canceled dont allow to charge quite yet
    /// </summary>
    private void DoneCanceling()
    {
        _canCharge = false;
    }
}

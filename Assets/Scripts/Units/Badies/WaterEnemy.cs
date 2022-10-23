using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEnemy : BadGuyUnitBase
{
    public Transform _leftTarget, _rightTarget, _currentTarget;
    private Vector2 _startPos, _endPos;
    public Vector2 _burstDistance = new Vector2(2, 0);
    public float _burstTime = 1;
    private Timer _lerpTimer;
    private float _currentTime;
    private bool _slowing = false;

    public enum Direction
    {
        Right = 0,
        Left = 1
    }
    public Direction _dir;

    protected override void Awake()
    {
        base.Awake();
        _animator = gameObject.GetComponent<Animator>();
        _lerpTimer = gameObject.AddComponent<Timer>();
        _lerpTimer.OnTimerCompleted += StartLerp;
        if (_dir == Direction.Left)
        {
            _currentTarget = _leftTarget;
        }else if (_dir == Direction.Right)
        {
            _currentTarget = _rightTarget;
        }
    }

    private void OnDestroy()
    {
        _lerpTimer.OnTimerCompleted -= StartLerp;
    }
    private void Start()
    {
        StartLerp();
    }

    /// <summary>
    /// Lerps enemy units position towards target position and changes direction after if it has moved to max position
    /// </summary>
    private void Update()
    {
        
        if (_dir == Direction.Left && _currentTarget == _leftTarget && transform.position.x <= _currentTarget.position.x || 
            _dir == Direction.Right && _currentTarget == _rightTarget && transform.position.x >= _currentTarget.position.x) 
        { 
            SwitchTarget();
            StartLerp();
        }
            
        
        if (_lerpTimer._isRunning)
        {
            _currentTime = _lerpTimer.NormalizedTimeElapsed;
            _currentTime = Mathf.Sin(_currentTime * Mathf.PI * 0.55f);
            transform.position = Vector2.Lerp(_startPos, _endPos, _currentTime);
            if(_lerpTimer.NormalizedTimeElapsed > 0.75f && _slowing == false){
                _animator.SetTrigger("Slow");
                _slowing = true;
            }
        }
    }

    /// <summary>
    /// determines how far unit will move and start lerp timer that determines move length
    /// </summary>
    private void StartLerp()
    {
        _slowing = false;
        if (_lerpTimer._isRunning == false)
        { 
            _currentTime = 0; 
        }
        _startPos = transform.position;
        if (_dir == Direction.Left)
        {
            _endPos = _startPos - _burstDistance;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            _endPos = _startPos + _burstDistance;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        _lerpTimer.StartTimer(_burstTime);
    }

    private void SwitchTarget()
    {
        if (_currentTarget == _leftTarget)
        {
            _currentTarget = _rightTarget;
        }else
        {
            _currentTarget = _leftTarget;
        }
        
        if(_dir == Direction.Left)
        {
            _dir = Direction.Right;
        }else
        {
            _dir = Direction.Left;
        }
        
    }

}

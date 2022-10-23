using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vomiter : BadGuyUnitBase
{
    private PukeBall _pukeBall;
    public int _pukeAmount = 20, _speed = 1, _random = 0, _previousRandom = 0;
    public float _pukeInterval = 5, _shortenedPukeInterval = 1; 
    private float _ogPukeInterval;
    public bool _move = false;
    private RepeatingTimer _repeatPukeTimer;
    public PukePool _pukePool;
    public List<Transform> _movePositions = new List<Transform>();
    private Transform _moveHere;
    private Timer _pukeTimer;

    private void OnDestroy()
    {
        _repeatPukeTimer.OnTimerCompleted -= Puke;
        _pukeTimer.OnTimerCompleted -= TimeToPuke;
        _hitTimer.OnTimerCompleted -= GetMoveTarget;
    }

    protected override void Awake()
    {
        base.Awake();
        _ogPukeInterval = _pukeInterval;
        _pukeTimer = gameObject.AddComponent<Timer>();
        _repeatPukeTimer = gameObject.AddComponent<RepeatingTimer>();
        _repeatPukeTimer.OnTimerCompleted += Puke;
        _pukeTimer.OnTimerCompleted += TimeToPuke;
        _hitTimer.OnTimerCompleted += GetMoveTarget;
        GetMoveTarget();
    }

    private void Update()
    {
        if (_move) {
            Move();
        }else if (_pukeTimer._isRunning == false && _repeatPukeTimer._isRunning == false && _hitTimer._isRunning == false)
        {
            GetMoveTarget();
        }
        if (_repeatPukeTimer.HasCompleted == true)
        {
            _animator.SetBool("Vomit", false);
        }

    }
    /// <summary>
    /// Moves to appointed position and stops when close enough
    /// </summary>
    private void Move()
    {
        if (Vector2.Distance(transform.position, _moveHere.position) > 0.1f) 
        { 
            transform.position = Vector2.MoveTowards(transform.position, _moveHere.position, _speed * Time.deltaTime);
        }
        else
        {
            StopMove();
        }
    }

    /// <summary>
    /// Stop moving and start puking
    /// </summary>
    private void StopMove()
    {
        _move = false;
        if (_pukeTimer._isRunning == false)
        {
            _pukeTimer.StartTimer(_pukeInterval);
            if (_pukeInterval == _shortenedPukeInterval) _pukeInterval = _ogPukeInterval;
        }
    }

    /// <summary>
    /// Starts looping timer that determines how frequently pukeballs are made
    /// </summary>
    private void TimeToPuke()
    {
        _repeatPukeTimer.StartTimer(0.05f, _pukeAmount);
    }

    /// <summary>
    /// Appoints target to move towards
    /// </summary>
    private void GetMoveTarget()
    {
        while (_random == _previousRandom)
        {
            _random = Random.Range(0,_movePositions.Count);
        }
        _previousRandom = _random;
        _moveHere = _movePositions[_random];
        _move = true;
    }


    /// <summary>
    /// Every time looping timer is completer make a pukeball that hurts the player
    /// </summary>
    private void Puke()
    {
        _animator.SetBool("Vomit", true);
        _pukeBall = _pukePool.GetPooledObject();
        _pukeBall.transform.position = transform.position + new Vector3(Random.Range(-0.1f, 0.1f), -1.5f, 0);
    }

    /// <summary>
    /// Gets hurt and stops everything briefly when hit by flame
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (collision.gameObject.GetComponent<Flame>() != null)
            {
                StopMove();
                _repeatPukeTimer.StopTimer();
                _repeatPukeTimer.HasCompleted = true;
                _pukeTimer.StopTimer();
                _animator.SetBool("Vomit", false);
                _pukeInterval = _shortenedPukeInterval;
            }
        }
        base.OnCollisionEnter2D(collision);
    }
}

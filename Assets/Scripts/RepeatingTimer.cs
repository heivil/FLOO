using System.Collections;
using System;
using UnityEngine;

public class RepeatingTimer : MonoBehaviour
{
    private float _time = 0.0f;
    private float _currentTime;
    public bool _isRunning = false;
    private bool _hasCompleted = false;
    public event Action OnTimerCompleted;
    public int _timesCompleted = 0;
    public int _runCount = 0;

    public bool HasCompleted
    {
        get
        {
            return _hasCompleted;
        }
        set
        {
            _hasCompleted = value;
        }
    }

    public float CurrentTime
    {
        get { return _currentTime; }
    }

    void Update()
    {
        if (_currentTime < _time && _isRunning)
        {
            _currentTime += Time.deltaTime;
        }
        else if (_isRunning)
        {
            CompleteTimer();
        }
    }

    public void StartTimer(float time, int count)
    {
        _runCount = count;
        _time = time;
        _currentTime = 0;
        _isRunning = true;
        _hasCompleted = false;
        _timesCompleted = 0;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void CompleteTimer()
    {
        _isRunning = false;
        OnTimerCompleted?.Invoke();
        _timesCompleted++;
        if(_timesCompleted < _runCount)
        {
            Repeat();
        }
        else
        {
            TrulyCompleted();
        }
    }
    private void TrulyCompleted()
    {
        _hasCompleted = true;
    }

    private void Repeat()
    {
        _currentTime = 0;
        _isRunning = true;
    }

    protected virtual void OnDestroy()
    {
        OnTimerCompleted = null;
    }
}


using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    private float _time = 0.0f;
    private float _currentTime = 0;
    public bool _isRunning = false;
    private bool _hasCompleted = false;
    public event Action OnTimerCompleted;

    public bool HasCompleted
    {
        get
        {
            return _hasCompleted;
        }
    }

    public float CurrentTime
    {
        get { return _currentTime; }
        private set { _currentTime = value; }
    }

    public float NormalizedTimeElapsed
    {
        get { return _currentTime / _time; }
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

    public void StartTimer(float time)
    {
        _time = time;
        _currentTime = 0.0f;
        _isRunning = true;

    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void CompleteTimer()
    {
        _isRunning = false;
        _hasCompleted = true;
        OnTimerCompleted?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        OnTimerCompleted = null;
    }
}
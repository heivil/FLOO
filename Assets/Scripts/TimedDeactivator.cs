using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeactivator : MonoBehaviour
{
    private Timer _timer;
    public float _time;

    private void Awake()
    {
        _timer = gameObject.AddComponent<Timer>();
        _timer.OnTimerCompleted += Deactivate;
    }

    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= Deactivate;
    }

    private void OnEnable()
    {
        _timer.StartTimer(_time);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

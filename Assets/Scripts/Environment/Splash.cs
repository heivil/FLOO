using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    private Timer _timer;
    public float _lifeTime;

    private void Awake()
    {
        _timer = gameObject.AddComponent<Timer>();
        _timer.OnTimerCompleted += Dissappear;
    }

    private void OnEnable()
    {
        _timer.StartTimer(_lifeTime);
    }
    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= Dissappear;
    }

    private void Dissappear()
    {
        gameObject.SetActive(false);
    }

}

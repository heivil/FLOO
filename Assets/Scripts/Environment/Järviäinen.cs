using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Järviäinen : MonoBehaviour
{
    private Animator _animator;
    private Timer _timer;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _timer = gameObject.AddComponent<Timer>();
        _timer.OnTimerCompleted += PlayAnimation;
    }

    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= PlayAnimation;
    }


    private void Update()
    {
        if (_timer._isRunning == false)
        {
            float random = Random.Range(15.0f, 30.0f);
            _timer.StartTimer(random);
        }
    }

    private void PlayAnimation()
    {
        _animator.SetTrigger("Go");
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBurn : MonoBehaviour
{
    private Timer _lifeTimer;
    public float _burnTime = 1;
    private int _damage;

    private void Awake()
    {
        _lifeTimer = gameObject.AddComponent<Timer>();
        _lifeTimer.OnTimerCompleted += GoOut;
        if (GameManager.Instance.LessDamage)
        {
            _damage = 4;
        }else
        {
            _damage = 5;
        }
    }
    private void OnEnable()
    {
        if (_lifeTimer._isRunning == false)
        {
            _lifeTimer.StartTimer(_burnTime);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _lifeTimer.StopTimer();
    }

    private void OnDestroy()
    {
        _lifeTimer.OnTimerCompleted -= GoOut;
    }


    public void GoOut()
    {
        gameObject.transform.GetComponentInParent<GooHitDetector>().TakeTheHit(_damage, false, true);
        gameObject.SetActive(false);

    }
}

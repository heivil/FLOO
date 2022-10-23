using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PukeBall : MonoBehaviour
{
    private Timer _timer;
    public int _lifeTime = 10;
    private Animator _animator;
    private Rigidbody2D _rB;
    private float _maxVelocity = 10;

    private void Awake()
    {
        _timer = gameObject.AddComponent<Timer>();
        _timer.OnTimerCompleted += AwayAnimation;
        _animator = gameObject.GetComponent<Animator>();
        _rB = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        float randomTimeAddition = Random.Range(0.0f,1.0f);    
        _timer.StartTimer(_lifeTime + randomTimeAddition);
    }
    private void FixedUpdate()
    {
        _rB.velocity = Vector3.ClampMagnitude(_rB.velocity, _maxVelocity);
    }
    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= AwayAnimation;
    }

    private void AwayAnimation()
    {
        _animator.SetTrigger("Away");
    }

    public void GoAway()
    {
        gameObject.SetActive(false);
    }
}

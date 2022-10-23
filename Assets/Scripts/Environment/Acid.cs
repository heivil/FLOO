using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    private Rigidbody2D _rB;
    public float _maxVelocity = 10, _lifeTime = 30;
    private Timer _timer;
    private Animator _animator;
    private float _animRandomizer;
    private void Awake()
    {
        _rB = gameObject.GetComponent<Rigidbody2D>();
        _timer = gameObject.AddComponent<Timer>();
        _timer.OnTimerCompleted += AwayAnimation;
        _animator = gameObject.GetComponent<Animator>();
        _animRandomizer = Random.Range(0.0f , 0.9f);
    }

    private void OnEnable()
    {
        float randomTimeAddition = Random.Range(0.0f, 1.0f);
        _timer.StartTimer(_lifeTime + randomTimeAddition);
        _animator.Play("Base Layer.Acid", 0, _animRandomizer);
    }
    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= AwayAnimation;
    }
    private void FixedUpdate()
    {
        _rB.velocity = Vector3.ClampMagnitude(_rB.velocity, _maxVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rope"))
        {
            collision.gameObject.GetComponent<Rope>().Snap();
        }
        else if (collision.gameObject.layer == 9)
        {
            collision.gameObject.GetComponent<BadGuyUnitBase>().Die();
        }
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

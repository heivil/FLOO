using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenContainerHalf : MonoBehaviour
{
    private Rigidbody2D _rb;
    public Vector2 _addedForce = new Vector2(1,0);
    private Timer _timer;
    public float _lifeTime = 2;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _timer = gameObject.AddComponent<Timer>();
        _timer.OnTimerCompleted += GoAway;
    }

    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= GoAway;
    }

    private void OnEnable()
    {
        _rb.AddForce(_addedForce);
        _timer.StartTimer(_lifeTime);
    }

    private void GoAway()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpShooter : BadGuyUnitBase
{
    [HideInInspector]
    public Timer _shootTimer;
    private UpShot _shot;
    public float _shotInterval = 3, _startDelay = 1;

    protected override void Awake()
    {
        base.Awake();
        _shootTimer = gameObject.AddComponent<Timer>();
        _shot = GetComponentInChildren<UpShot>();
        _shootTimer.OnTimerCompleted += Shoot;
    }

    private void OnDestroy()
    {
        _shootTimer.OnTimerCompleted -= Shoot;
    }

    private void Start()
    {
        _shootTimer.StartTimer(_startDelay);
        _shot.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shoots projectile upwards, unparents shot so it wont turn with unit if unit falls or turns
    /// </summary>
    private void Shoot()
    {
        _shot.transform.parent = null;
        _shot._startPos = transform.position + transform.up * 0.35f;
        _shot._targetPosition = _shot._startPos + (Vector2)transform.up * _shot._targetDistance;
        _shot.gameObject.SetActive(true);
        _animator.SetTrigger("Shoot");
    }

    public override void Die()
    {
        base.Die();
    }
}
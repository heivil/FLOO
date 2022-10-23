using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : BadGuyUnitBase
{
    public BoxCollider2D _leftColl, _rightColl, _upColl, _downColl;
    private CapsuleCollider2D _collider;
    public SpriteRenderer _leftRend, _rightRend, _upRend, _downRend;
    public Transform[] _targets = new Transform[6];
    private Transform _attackTarget, _moveTarget;
    private bool _attacking;
    private Timer _attackTimer, _moveTimer, _tiredTimer, _withdrawTimer;
    public float _waitTime = 3, _attackTime = 8, _moveTime = 2, _vulnerableTime = 6;
    private Vector3 _startPosition;
    private Vector2 _realVertTarget;
    public Vector2 _midVertTarget = new Vector2(20, 18), _edgeVertTarget = new Vector2(20, 34), _horTarget = new Vector2(20, 24), _horColStart = new Vector2(3, 24), _vertColStart = new Vector2(20, 3); 
    private List<BoxCollider2D> _horColliders = new List<BoxCollider2D>(), _vertColliders = new List<BoxCollider2D>();
    private SpriteRenderer _renderer;

    protected override void Awake()
    {
        base.Awake();
        _attackTimer = gameObject.AddComponent<Timer>();
        _tiredTimer = gameObject.AddComponent<Timer>();
        _moveTimer = gameObject.AddComponent<Timer>();
        _withdrawTimer = gameObject.AddComponent<Timer>();
        _tiredTimer.OnTimerCompleted += GetTargets;
        _attackTimer.OnTimerCompleted += EndAttack;
        _moveTimer.OnTimerCompleted += StartWithdrawTimer;
        _withdrawTimer.OnTimerCompleted += Tired;
        _renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        _collider = gameObject.GetComponent<CapsuleCollider2D>();
        _hitTimer.OnTimerCompleted += GetTargets;
        _moveTarget = _targets[3];
    }

    private void OnDestroy()
    {
        _tiredTimer.OnTimerCompleted -= GetTargets;
        _attackTimer.OnTimerCompleted -= EndAttack;
        _moveTimer.OnTimerCompleted -= StartWithdrawTimer;
        _withdrawTimer.OnTimerCompleted -= Tired;
        _hitTimer.OnTimerCompleted -= GetTargets;
    }

    private void Update()
    {
        if (_attackTimer._isRunning)
        {
            Attack();
            StretchSprites();
        }else if (_moveTimer._isRunning)
        {
            MoveToTarget();
        }else if (_withdrawTimer._isRunning)
        {
            WithdrawAttack();
            StretchSprites();
        }
    }

    public void StartBossFight()
    {
        GetTargets();
    }

    /// <summary>
    /// Gets a new position to move to and attack to. Cannot attack the same position unit is already in. 
    /// Also cannot move to the attack position or the units current position
    /// </summary>
    private void GetTargets()
    {
        gameObject.layer = 14;
        _animator.SetBool("Attack", true);
        _animator.ResetTrigger("Hit");
        Transform newTarget = _targets[Random.Range(0, 6)];
        while (newTarget == _attackTarget || newTarget == _moveTarget)
        {
            newTarget = _targets[Random.Range(0, 6)];
        }
        _attackTarget = newTarget;

        newTarget = _targets[Random.Range(0, 6)];

        while (newTarget == _moveTarget || newTarget == _attackTarget)
        {
            newTarget = _targets[Random.Range(0, 6)];
        }
        _moveTarget = newTarget;
        PlanAttack();
    }

    /// <summary>
    /// Sets attack position and limits for the damaging smoke to move to
    /// </summary>
    private void PlanAttack()
    {
        _horColliders.Clear();
        _vertColliders.Clear();
        if (_attackTarget == _targets[0])
        {
            _horColliders.Add(_rightColl);
            _vertColliders.Add(_downColl);
            _realVertTarget = _edgeVertTarget;
        }
        else if (_attackTarget == _targets[1])
        {
            _horColliders.Add(_leftColl);
            _vertColliders.Add(_downColl);
            _realVertTarget = _edgeVertTarget;
        }
        else if (_attackTarget == _targets[2])
        {
            _vertColliders.Add(_downColl);
            _vertColliders.Add(_upColl);
            _horColliders.Add(_rightColl);
            _realVertTarget = _midVertTarget;
        }
        else if (_attackTarget == _targets[3]) 
        {
            _vertColliders.Add(_downColl);
            _vertColliders.Add(_upColl);
            _horColliders.Add(_leftColl);
            _realVertTarget = _midVertTarget;
        }
        else if (_attackTarget == _targets[4])
        {
            _horColliders.Add(_rightColl);
            _vertColliders.Add(_upColl);
            _realVertTarget = _edgeVertTarget;
        }
        else if(_attackTarget == _targets[5])
        {
            _horColliders.Add(_leftColl);
            _vertColliders.Add(_upColl);
            _realVertTarget = _edgeVertTarget;
        }
        _attackTimer.StartTimer(_attackTime);
    }

    /// <summary>
    /// Starts attack and the timer that determines lenght and speed of attack
    /// </summary>
    private void Attack()
    {
        for(int i = 0; i < _vertColliders.Count; i++)
        {
            _vertColliders[i].size = Vector2.Lerp(_vertColStart, _realVertTarget, _attackTimer.NormalizedTimeElapsed);
        }
        _horColliders[0].size = Vector2.Lerp(_horColStart, _horTarget, _attackTimer.NormalizedTimeElapsed);
    }

    /// <summary>
    /// After attack start moving to new position 
    /// </summary>
    private void EndAttack()
    {
        _startPosition = transform.position;
        
        _moveTimer.StartTimer(_moveTime);
    }
    /// <summary>
    /// Lerp to the new target. Go through everything
    /// </summary>
    private void MoveToTarget()
    {
        _collider.enabled = false;
        _renderer.enabled = false;
        transform.position = Vector3.Lerp(_startPosition, _moveTarget.position, _moveTimer.NormalizedTimeElapsed);
    }

    /// <summary>
    /// Enable colliders and start timer to withdraw smoke
    /// </summary>
    private void StartWithdrawTimer()
    {
        
        _collider.enabled = true;
        _renderer.enabled = true;
        _withdrawTimer.StartTimer(_attackTime/2);
    }

    /// <summary>
    /// Return damaging smoke to original position
    /// </summary>
    private void WithdrawAttack()
    {
        for (int i = 0; i < _vertColliders.Count; i++)
        {
            _vertColliders[i].size = Vector2.Lerp(_realVertTarget, _vertColStart, _withdrawTimer.NormalizedTimeElapsed);
        }
        _horColliders[0].size = Vector2.Lerp(_horTarget, _horColStart, _withdrawTimer.NormalizedTimeElapsed);
    }

    /// <summary>
    /// Set tired state when player is able to attack 
    /// </summary>
    private void Tired()
    {
        _animator.SetBool("Attack", false);
        _animator.SetBool("Tired", true);
        gameObject.layer = 9;
        if (_tiredTimer._isRunning == false && _hitTimer._isRunning == false)
        {
            _tiredTimer.StartTimer(_vulnerableTime);
        }
    }

    /// <summary>
    /// Streches damaging smokes sprites so the somke cover almost the entire level according to attack
    /// </summary>
    private void StretchSprites()
    {
        _leftRend.size = _leftColl.size + Vector2.right;
        _rightRend.size = _rightColl.size + Vector2.right;
        _upRend.size = _upColl.size + Vector2.up;
        _downRend.size = _downColl.size + Vector2.up;
    }

    /// <summary>
    /// Stop being tired if being damaged by player
    /// </summary>
    /// <param name="collision">Collider of other game object</param>
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Flame>() != null && _animator.GetBool("Tired"))
        {
            _animator.SetBool("Tired", false);
            if (_tiredTimer._isRunning) _tiredTimer.StopTimer();
            base.OnCollisionEnter2D(collision);
        }
    }

}

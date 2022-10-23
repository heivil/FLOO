using UnityEngine;

public class Flyer : BadGuyUnitBase, ITargeter
{
    [SerializeField, Tooltip("do not insert transforms by hand, this is done in code")]
    private Transform[] _targets = new Transform[2];
    private int _targetCounter = 0;
    private Timer _noticeTimer, _waitTimer;
    private Transform _currentTarget, _attackTarget;
    public float _attackDelay = 0.5f, _chargeSpeed = 1, _stopAttackMargin = 0.5f, _flySpeed = 1, _waitTime = 3;
    private SpriteRenderer _renderer;
    private bool _attacking = false, _flyingForward = true, _flying = true;

    public enum FlyMode
    {
        Loop = 0,
        BackAndForth = 1
    }
    public FlyMode _flyMode;

    protected override void Awake()
    {
        base.Awake();
        _noticeTimer = gameObject.AddComponent<Timer>();
        _waitTimer = gameObject.AddComponent<Timer>();
        //make sure "Targets" GameObject is the third child under the parent prefab!!!!!!!!!!!!!
        for (int i = 0; i < _targets.Length; i++)
        {
            _targets[i] = transform.parent.GetChild(2).transform.GetChild(i);
        }
        _currentTarget = _targets[0];
        _noticeTimer.OnTimerCompleted += Attack;
        _waitTimer.OnTimerCompleted += AttackAgain;
        _rB = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        _noticeTimer.OnTimerCompleted -= Attack;
        _waitTimer.OnTimerCompleted -= AttackAgain;
    }

    /// <summary>
    /// If not attacking, flies either back and forth or in a loop. If attacking flies toward player
    /// </summary>
    void Update()
    {
        if (_flying)
        {
            _rB.gravityScale = 0;
            _rB.velocity = Vector2.zero;
            if (_attacking == false && _noticeTimer._isRunning == false)
            {
                if (Vector2.Distance(transform.position, _currentTarget.position) < 0.1f)
                {
                    SwitchTarget();
                }
                transform.position = Vector2.MoveTowards(transform.position, _currentTarget.position, _flySpeed * Time.deltaTime);
                LookAtThis(_currentTarget);
            }
            else if (_attacking == true && _attackTarget != null)
            {
                if (transform.position.y + _stopAttackMargin < _attackTarget.transform.position.y)
                {
                    _attacking = false;
                    _animator.SetBool("Attack", false);
                    _waitTimer.StartTimer(_waitTime);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, _attackTarget.position, _chargeSpeed * Time.deltaTime);
                    LookAtThis(_attackTarget);
                }
            }
        }
        else
        {
            _rB.gravityScale = 1;
            if(_rB.velocity.magnitude < 0.1f)
            {
                _flying = true;
            }
        }
    }

    /// <summary>
    /// Swithces target to fly towards to
    /// </summary>
    private void SwitchTarget()
    {
        if (_flyMode == FlyMode.Loop) 
        {
            _targetCounter++;
            if (_targetCounter < _targets.Length)
            {
                _currentTarget = _targets[_targetCounter];
            }
            else
            {
                _targetCounter = 0;
                _currentTarget = _targets[_targetCounter];
            }
        }
        else if (_flyMode == FlyMode.BackAndForth)
        {
            if (_flyingForward == true)
            {
                _targetCounter++;
            }
            else if(_flyingForward == false)
            {
                _targetCounter--;
            }
            
            if (_targetCounter < _targets.Length && _flyingForward == true || _targetCounter >= 0 && _flyingForward == false)
            {
                _currentTarget = _targets[_targetCounter];
            }
            else if (_targetCounter >= _targets.Length)
            {
                _flyingForward = false;
                _targetCounter = _targetCounter - 2;
                _currentTarget = _targets[_targetCounter];
            } 
            else if (_flyingForward == false && _targetCounter < 0)
            {
                _flyingForward = true;
                _targetCounter = _targetCounter + 2;
                _currentTarget = _targets[_targetCounter];
            }
        }
    }

    /// <summary>
    /// Makes unit look at the target it is flying towards
    /// </summary>
    /// <param name="lookHere">Transform to look at</param>
    private void LookAtThis(Transform lookHere)
    {
        transform.LookAt(new Vector3(lookHere.position.x, transform.position.y, transform.position.z));
        transform.rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
    }

    /// <summary>
    /// Targets player when they come in range
    /// </summary>
    /// <param name="player">Player gameobject</param>
    public void Target(GameObject player)
    {
        _attackTarget = player.transform;
        if (player.gameObject.transform.position.y < transform.position.y + _stopAttackMargin)
        {
            _animator.SetBool("Attack", true);
            _noticeTimer.StartTimer(_attackDelay);
        }else
        {
            _waitTimer.StartTimer(1);
        }
    }

    /// <summary>
    /// Stops unit from attacking player once player is out of range
    /// </summary>
    public void ClearTarget()
    {
        _attacking = false;
        _attackTarget = null;
        _animator.SetBool("Attack", false);
        _noticeTimer.StopTimer();
    }

    private void Attack()
    {
        _attacking = true;
    }

    /// <summary>
    /// attacks again after set period of time if player is still in range
    /// </summary>
    private void AttackAgain()
    {
        if (_attackTarget != null)
        {
            Target(_attackTarget.gameObject);
        }
    }

    /// <summary>
    /// Stops attacking for a while when hitting the player
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == 12)
        {
            _attacking = false;
            _noticeTimer.StopTimer();
            _animator.SetBool("Attack", false);
            _waitTimer.StartTimer(_waitTime);
            SwitchTarget();
        }
    }

    /// <summary>
    /// falss down when shot with a cannon ball
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            _flying = false;
            _attacking = false;
            _attackTarget = null;
        }
    }
}

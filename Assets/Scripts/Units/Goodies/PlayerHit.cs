using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHit : MonoBehaviour
{
    protected Timer _immortalityTimer;
    protected bool _visible = true;
    //public float _flashIntervalTime = 0.2f;
    //public int _flashAmount = 6;
    protected Collider2D _hitCollider;
    protected SpriteRenderer _renderer;
    public int _damageAmount;
    protected FriendlyUnitBase _friendlyUnit;
    protected Animator _animator;
    public Vector2 _pushBack = new Vector2(20, 20);
    protected Vector2 _pushDirection;

    protected virtual void Awake()
    {
        _immortalityTimer = GameManager.Instance.LevelManager._player._immortalityTimer;
        _hitCollider = GetComponent<Collider2D>();
        _friendlyUnit = gameObject.GetComponentInParent<FriendlyUnitBase>();
        _immortalityTimer.OnTimerCompleted += UnHit;
        if (GameManager.Instance.LessDamage)
        {
            _damageAmount = 8;
        }else
        {
            _damageAmount = 10;
        }
        if (gameObject.transform.parent.gameObject.GetComponentInChildren<Animator>() != null) 
        { 
            _animator = gameObject.transform.parent.gameObject.GetComponentInChildren<Animator>();
        }
        else
        {
            Debug.LogError("null animator from playerhit shit ");
        }
    }
    private void OnEnable()
    {
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }

    private void OnDisable()
    {
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }

    private void OnDestroy()
    {
        _immortalityTimer.OnTimerCompleted -= UnHit;
    }

    /// <summary>
    /// Pushes player back a little when hitting a damaging obstacle
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14  || collision.gameObject.layer == 17 || collision.gameObject.layer == 20 )
        {
            if (collision.transform.position.x < transform.position.x)
            {
                _pushDirection = new Vector2(1,1);
            }
            else
            {
                _pushDirection = new Vector2(-1, 1);
            }

            TakeTheHit(_damageAmount, true);
        }

    }

    /// <summary>
    /// If player stays on damaging obstacle, determines if they should take damage based on type of obstacle
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            TakeTheHit(_damageAmount, false);
        }else if (collision.gameObject.layer == 17 || collision.gameObject.layer == 20)
        {
            TakeTheHit(_damageAmount, true);
        }

    }


    /// <summary>
    /// After brief immortality after taking hit, enables player to get hit again
    /// </summary>
    private void UnHit()
    {
        if (_animator.GetBool("Hit") == true)
        {
            _animator.SetBool("Hit", false);
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);

    }

    /// <summary>
    /// Determines damage taken and if player should be pushed away from enemy or not
    /// </summary>
    /// <param name="damage">Amount of damage taken</param>
    /// <param name="pushBack">Determines if player is pushed away or not</param>
    public virtual void TakeTheHit(int damage, bool pushBack)
    {
        if (_immortalityTimer._isRunning == false && GameManager.Instance.LevelManager._levelEnded == false)
        {
            _immortalityTimer.StartTimer(2);
            _friendlyUnit.ReduceGreen(damage);
            _friendlyUnit._greenChange.gameObject.SetActive(false);
            _friendlyUnit._greenChange.text = (-damage).ToString();
            _friendlyUnit._greenChange.color = GameManager.Instance.LevelManager._player._red;
            _friendlyUnit._greenChange.gameObject.SetActive(true);
            if (GameManager.Instance.LevelManager._player.Alive)
            {
                _friendlyUnit._audioSource.PlayOneShot(_friendlyUnit._hitSound);
                if (pushBack) 
                {
                    _friendlyUnit.transform.rotation = Quaternion.Euler(0, 0, 0);
                    _friendlyUnit._rB.velocity = Vector2.zero;
                    _friendlyUnit._rB.AddForce(_pushBack * _pushDirection);
                }
            }
            _friendlyUnit._rB.gravityScale = _friendlyUnit._gravityScale;
        }
    }

    /// <summary>
    /// Determines damage taken and if player should be pushed away from enemy or not
    /// </summary>
    /// <param name="damage">Amount of damage taken</param>
    /// <param name="pushBack">Determines if player is pushed away or not</param>
    /// <param name="ignoreImmortality">Depending on damage type, determines wheter to ignore brief immortality after taking damage</param>
    public virtual void TakeTheHit(int damage, bool pushBack, bool ignoreImmortality)
    {
        if (_immortalityTimer._isRunning == false || ignoreImmortality)
        {
            if (GameManager.Instance.LevelManager._levelEnded == false)
            {
                _immortalityTimer.StartTimer(2);
                _friendlyUnit.ReduceGreen(damage);
                _friendlyUnit._greenChange.gameObject.SetActive(false);
                _friendlyUnit._greenChange.text = (-damage).ToString();
                _friendlyUnit._greenChange.color = GameManager.Instance.LevelManager._player._red;
                _friendlyUnit._greenChange.gameObject.SetActive(true);
                if (GameManager.Instance.LevelManager._player.Alive)
                {
                    _friendlyUnit._audioSource.PlayOneShot(_friendlyUnit._hitSound);
                    if (pushBack)
                    {
                        _friendlyUnit.transform.rotation = Quaternion.Euler(0, 0, 0);
                        _friendlyUnit._rB.velocity = Vector2.zero;
                        _friendlyUnit._rB.AddForce(_pushBack * _pushDirection);
                    }
                }
                _friendlyUnit._rB.gravityScale = _friendlyUnit._gravityScale;
            }
        }
    }
}

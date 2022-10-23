using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuyUnitBase : MonoBehaviour
{
    public GameObject _burn, _deathSmoke;
    public int _hitPoints = 1;
    protected Animator _animator;
    protected Timer _hitTimer;  
    public Timer _endDelayTimer;
    [SerializeField]
    protected float _hitTime = 1.0f, _endTime = 3;
    public bool _endLevel;
    public float _maxVelocity = 10, _maxBounceVelocity = 15;
    public Rigidbody2D _rB;
    public AudioSource _audiosource;
    public AudioClip _deathSound, _hitSound;

    protected virtual void Awake()
    {
        if (_endLevel &&  _endDelayTimer != null)
        {
            _endDelayTimer.OnTimerCompleted += EndLevel;
        }
        _animator = gameObject.GetComponentInChildren<Animator>();
        _hitTimer = gameObject.AddComponent<Timer>();
    }
    private void OnDestroy()
    {
        if (_endDelayTimer != null)
        {
            _endDelayTimer.OnTimerCompleted -= EndLevel;
        }
    }
    protected virtual void FixedUpdate()
    {
        if(_rB != null)
        {
            if (_rB.velocity.y > _maxVelocity)
            {
                _rB.velocity = Vector3.ClampMagnitude(_rB.velocity, _maxBounceVelocity);
            }
            else
            {
                _rB.velocity = Vector3.ClampMagnitude(_rB.velocity, _maxVelocity);
            }
        }
    }
    public virtual void Die()
    {
        _audiosource.PlayOneShot(_deathSound);
        _deathSmoke.transform.rotation = Quaternion.Euler(0, 0, 0);
        _deathSmoke.SetActive(true);
        _deathSmoke.gameObject.transform.parent = null;    
        _audiosource.gameObject.transform.parent = null;
        if (_endLevel)
        {
            GameManager.Instance.LevelManager._levelEnded = true;
            _endDelayTimer.StartTimer(1);
            _endDelayTimer.gameObject.transform.parent = null;
        }
       
        gameObject.SetActive(false); 
    }

    /// <summary>
    /// Ends level after boss is beaten
    /// </summary>
    private void EndLevel()
    {
        GameManager.Instance.LevelManager.StartEndFeedback();
    }

    /// <summary>
    /// When ememy is hit by flame (player) or spikes, small enemies die in 1 hit, bosses take damage and die in 3 hits.
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Flame>() != null && _hitTimer._isRunning == false && collision.gameObject.GetComponent<Flame>().CheckIfHit() == false)
        {   
            _hitPoints--;
            if (_hitPoints <= 0)
            {
                _burn.transform.rotation = Quaternion.Euler(0, 0, 0);
                _burn.SetActive(true);
                _burn.gameObject.transform.parent = null;
                Die();
            }
            else
            {
                _animator.SetTrigger("Hit");
                if(_hitSound != null)_audiosource.PlayOneShot(_hitSound);
                _hitTimer.StartTimer(_hitTime);
                collision.gameObject.GetComponent<Flame>().BounceFormEnemy(transform);
            }
        } else if(collision.gameObject.layer == 17)
        {
            _hitPoints--;
            if (_hitPoints <= 0)
            {
                Die();
            }
            else
            {
                _animator.SetTrigger("Hit");
                if (_hitTimer._isRunning == false) _hitTimer.StartTimer(_hitTime);
            }
        }
        
    }
}

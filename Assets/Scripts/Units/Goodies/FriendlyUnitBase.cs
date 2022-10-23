using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class FriendlyUnitBase : MonoBehaviour, ICollect
{
    protected float _switchTouchDist = 1;
   
    public PlayerHit _playerHitDet;
    [HideInInspector]
    public SpriteRenderer _spriteRenderer;
    public float _maxVelocity = 10, _maxBounceVelocity = 15;
    public Rigidbody2D _rB;
    public Animator _animator;
    public float _gravityScale;
    public AudioSource _audioSource;
    private Vector2 _clampedPos;
    public bool _ignoreOneSidedPlatform;
    [SerializeField]
    protected float _controlsDeadZone = 5;
    public TMP_Text _greenChange;
    public bool _hidden = false;
    public AudioClip _hitSound, _deathSound;
    protected bool _firstTouchOnUI;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _audioSource = gameObject.GetComponentInChildren<AudioSource>();
        _greenChange = GameManager.Instance.LevelManager._player._greenChange;
    }

    /// <summary>
    /// Does not let player to go out of play area
    /// </summary>
    private void ClampPosition()
    {
        if (transform.position.x > 8.0f)
        {
            _clampedPos = transform.position;
            _clampedPos.x = 8.0f;
            transform.position = _clampedPos;
        }else if (transform.position.x < -8.0f)
        {
            _clampedPos = transform.position;
            _clampedPos.x = -8.0f;
            transform.position = _clampedPos;
        }

        if (transform.position.y > 10)
        {
            _clampedPos = transform.position;
            _clampedPos.y = 10;
            transform.position = _clampedPos;
        }else if (transform.position.y < -10)
        {
            _clampedPos = transform.position;
            _clampedPos.y = -10;
            transform.position = _clampedPos;
        }
    }
    protected virtual void Update()
    {
        ClampPosition();
        
    }
    public void Collect(int i)
    {
        GameManager.Instance.LevelManager.ChangeGreenAmount(i);
    }

    /// <summary>
    /// Reduces collected currency when taking a hit 
    /// </summary>
    /// <param name="reduceAmount"></param>
    public void ReduceGreen(int reduceAmount)
    {
        if (GameManager.Instance.LevelManager._levelEnded == false) 
        {
            GameManager.Instance.LevelManager.ChangeGreenAmount(-reduceAmount);
            if (GameManager.Instance.LevelManager.CollectedGreens <= 0)
            {
                Die();
            }
        }
    }

    /// <summary>
    /// player dies if they run out of collected currency
    /// </summary>
    public virtual void Die()
    {
        GameManager.Instance.AudioManager.StopMusic();
        GameManager.Instance.LevelManager._player.Alive = false;
        if (gameObject.GetComponentInChildren<PlayerHit>() != null)
        {
            gameObject.GetComponentInChildren<PlayerHit>().gameObject.SetActive(false);
        }
        _animator.SetBool("Hit", false);
        _audioSource.Stop();
        _audioSource.gameObject.transform.parent = null;
        _audioSource.PlayOneShot(_deathSound);
    }

    public void DeathMenu()
    {
        GameManager.Instance.LevelManager.DeathMenu();
    }


}

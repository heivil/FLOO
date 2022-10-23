using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : BadGuyUnitBase
{
    public Vector2 _jumpforce = new Vector2(20, 30);
    public float _intervalTime = 2;
    private Timer _intervalTimer;
    private SpriteRenderer _renderer;
    private bool _canTurn = true, _grounded;
    private JumperWallDetector _detector;
    private int _jumpLayerMask = 1 << 10 | 1 << 9 | 1 << 8 | 1 << 19;
    public Collider2D _thisCollider;
    public Vector2 _boxCastSize = new Vector2(0.7f, 0.2f);
    public Vector3 _boxCastStartPos = new Vector3(0, 0.65f, 0);
    
    protected override void Awake()
    {
        base.Awake();
        _intervalTimer = gameObject.AddComponent<Timer>();
        _intervalTimer.OnTimerCompleted += GetReadyToJump;
        _rB = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _detector = gameObject.GetComponentInChildren<JumperWallDetector>();
    }

    private void OnDestroy()
    {
        _intervalTimer.OnTimerCompleted -= GetReadyToJump;
    }

    private void Update()
    {
        RaycastHit2D downHit = Physics2D.BoxCast(transform.position - _boxCastStartPos, _boxCastSize, 0, Vector2.zero, 0, _jumpLayerMask);
        
        if (downHit.collider != null && downHit.collider != _thisCollider)
        {
            _grounded = true;
        }else
        {
            _grounded = false;
        }
    }

    /// <summary>
    /// Changes animation when unit lands after jump and touches ground and determines if unit is on jumpable surface and starts detecting obstacles
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (_grounded) 
        {
            if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9 || collision.gameObject.layer == 8 || collision.gameObject.layer == 19)
            {
                _detector.gameObject.SetActive(true);
                _animator.SetBool("Jump", false);
            }
        }
    }

    /// <summary>
    /// JUmps after set time has passed and adds small randomiser to jump force
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9 || collision.gameObject.layer == 8 || collision.gameObject.layer == 19)
        {
            if (_intervalTimer._isRunning == false)
            {
                float randomizer = Random.Range(0, 0.5f);
                _intervalTimer.StartTimer(_intervalTime + randomizer);
            }
        }
    }

    /// <summary>
    /// Changes jumping direction
    /// </summary>
    public void ChangeDir()
    {
        if (_canTurn == true)
        {
            _canTurn = false;
            _jumpforce = new Vector2(_jumpforce.x * -1, _jumpforce.y);
        }
    }

    /// <summary>
    /// Faces unit to jumping direction
    /// </summary>
    private void GetReadyToJump()
    {
        if (_jumpforce.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        _animator.SetBool("Jump", true);
    }

    /// <summary>
    /// Adds force to jump, resets rotation and stops detecting walls for a moment
    /// </summary>
    public void Jump()
    {
        _detector.gameObject.SetActive(false);
        _canTurn = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _rB.AddForce(_jumpforce);
    }
}

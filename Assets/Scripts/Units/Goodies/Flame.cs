using UnityEngine;
using UnityEngine.EventSystems;

public class Flame : FriendlyUnitBase
{
    public float _maxSpeed = 10, _consumeTime = 1, _takeOffSpeed = 6, _maxForce = 75, _minForce = 20, _turnSpeed = 10, _dist;
    private Vector3 _firstTouchPosition;
    public CircleCollider2D _hitTrigger;
    public CapsuleCollider2D _collider;
    public bool _flying, _avoidGroundCollision = false;
    private Vector2 _dir, _defaultColliderOffset, _flyingColliderOffset = new Vector2(0, 0.25f);
    public Timer _consumeTimer;
    public AudioClip _flyingSound, _crashSound;
    public int _greenConsumeAmount = 1;
    private AimArrow _aimArrow;
    private Timer _bounceTimer;
    private bool _limitTakeoffRotation, _canTakeOff;
    private int _killingFee;

    protected override void Awake()
    {
        base.Awake();
        _flying = false;
        _animator = GetComponentInChildren<Animator>();
        _consumeTimer = gameObject.AddComponent<Timer>();
        //_ogMoveSpeed = _moveSpeed;
        _rB = GetComponent<Rigidbody2D>();
        _gravityScale = _rB.gravityScale;
        _defaultColliderOffset = _hitTrigger.offset;
        if (_consumeTimer != null) _consumeTimer.OnTimerCompleted += ConsumeGreen; 
        _aimArrow = gameObject.GetComponentInChildren<AimArrow>();
        _aimArrow.gameObject.SetActive(false);
        _bounceTimer = gameObject.AddComponent<Timer>();
        if (_bounceTimer != null) _bounceTimer.OnTimerCompleted += TimeThrow;
        if (GameManager.Instance.LessImpactDamage)
        {
            _killingFee = 1;
        }
        else
        {
            _killingFee = 2;
        }
    }

    public bool CheckIfHit()
    {
        return _animator.GetBool("Hit");
    }
    private void OnEnable()
    {
        _flying = false;
        _avoidGroundCollision = false;
        _canTakeOff = true;
    }

    private void OnDestroy()
    {
        if (_consumeTimer != null)
        {
            _consumeTimer.OnTimerCompleted -= ConsumeGreen;
        }
        if (_bounceTimer != null)
        {
            _bounceTimer.OnTimerCompleted -= TimeThrow;
        }
    }

    private void OnDisable()
    {
        if (_consumeTimer != null)
        {
            _consumeTimer.StopTimer();
        }

        _flying = false;
        _rB.gravityScale = _gravityScale;
        _aimArrow.gameObject.SetActive(false);
    }


    /// <summary>
    /// Horribly long update method detects touch, determines action according to touch, changes animations, sets collider offset to work with switching sprite pivot points
    /// </summary>
    protected override void Update()
    {
        base.Update();
        if (GameManager.Instance._paused == false && GameManager.Instance.LevelManager._player.Alive == true)
        {
            Touch touch;
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (_flying != true && _animator.GetBool("TakeOff") == false && _animator.GetBool("Crash") == false && _animator.GetBool("Throw") == false 
                    && _animator.GetBool("Fly") == false && _animator.GetBool("SlighterTurn") == false && _animator.GetBool("Turn") == false && _animator.GetBool("Hit") == false)
                {
                    if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        _firstTouchPosition = touch.position;
                        _animator.SetBool("TakeOff", true);
                        _firstTouchOnUI = false;
                    }
                    else if(touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        _firstTouchPosition = touch.position;
                        _firstTouchOnUI = true;
                    }
                    else if(touch.phase == TouchPhase.Moved && _canTakeOff)
                    {
                        if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId) && _firstTouchOnUI || !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            _firstTouchPosition = touch.position;
                            _firstTouchOnUI = false;
                            _animator.SetBool("TakeOff", true);
                        }
                    }
                }
                else if ((GameManager.Instance.LevelManager.CollectedGreens > 1 || GameManager.Instance.NoFlyConsume) && _animator.GetBool("Crash") == false && _animator.GetBool("Throw") == false && _flying == true)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            _firstTouchOnUI = true;
                            _firstTouchPosition = touch.position;
                        }
                        else
                        {
                            _firstTouchPosition = touch.position;

                            if (_animator.GetBool("TakeOff") == false)
                            {
                                _flying = true;
                                _rB.gravityScale = 0;
                            }
                        }
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId) && _firstTouchOnUI || 
                            !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            if (_animator.GetBool("TakeOff") == false)
                            {
                                _firstTouchOnUI = false;
                                _rB.gravityScale = 0;
                            }
                        }
                        
                    }
                }
                else if (_animator.GetBool("Crash") == true)
                {
                    _flying = false;
                    _consumeTimer.StopTimer();
                }
                else if (_flying && GameManager.Instance.LevelManager.CollectedGreens <= 1 && _animator.GetBool("Hit") == false && !GameManager.Instance.NoFlyConsume)
                {
                    _rB.gravityScale = _gravityScale;
                    _consumeTimer.StopTimer();
                    GameManager.Instance.LevelManager._canvas.StartFlash();
                }
                if(touch.phase == TouchPhase.Ended && _canTakeOff == false)
                {
                    _canTakeOff = true;
                }
            }
            else
            {
                _aimArrow.gameObject.SetActive(false);
            }  

            if (_flying)
            {
                _collider.offset = _flyingColliderOffset;
                _hitTrigger.offset = _flyingColliderOffset;
                TurnInFlight();
            }
            else if (_animator.GetBool("Crash") == true)
            {
                _collider.offset = _flyingColliderOffset;
                _hitTrigger.offset = _flyingColliderOffset;
            }
            else if (_animator.GetBool("TakeOff") == true)
            {
                _collider.offset = _defaultColliderOffset;
                _hitTrigger.offset = _defaultColliderOffset;
                _rB.gravityScale = 1;
                RotateAtTakeOff();
            }
            else
            {
                _collider.offset = _defaultColliderOffset;
                _hitTrigger.offset = _defaultColliderOffset;
            }
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance._paused == false)
        {
            if (_flying)
            {
                Fly();
            }

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

    /// <summary>
    /// Allows player rotation while player is about to fly. Limits rotation if on ground so player cannot turn up side down and crash immediately
    /// </summary>
    private void RotateAtTakeOff()
    {
        float angle;
        Quaternion angleAxis;
        if (Input.touchCount > 0 && _flying == false && _animator.GetBool("TakeOff") == true)
        {
            _animator.SetBool("Crash", false);
            Touch touch = Input.GetTouch(0);
            Vector3 v3TouchPos = touch.position;
            _dir = v3TouchPos - _firstTouchPosition;
            if (_dir.magnitude > _controlsDeadZone)
            {
                angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg - 90;

                if (_limitTakeoffRotation)
                {
                    if (angle >= -270 && angle <= -180)
                    {
                        angle = 60;
                    }
                    else if (angle <= -80 && angle > -180)
                    {
                        angle = -60;
                    }
                }
                angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = angleAxis;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0,0,0); 
            }

            
        }
        
    }

    /// <summary>
    /// Removes gravity for flight and sets initial direction for flight. Happens after take off animation is complete
    /// </summary>
    public void AfterTakeOff()
    {
        _flying = true;
        _animator.SetBool("TakeOff", false);
        _animator.SetBool("Fly", true);
        _rB.gravityScale = 0;
        if (_consumeTimer._isRunning == false && !GameManager.Instance.NoFlyConsume)
        {
            _consumeTimer.StartTimer(_consumeTime);
        }
        _rB.velocity = transform.up * _takeOffSpeed;
        _limitTakeoffRotation = false;
        _canTakeOff = false;
    }

    /// <summary>
    /// Manages flying velocity and direction
    /// </summary>
    private void Fly()
    {
        if (_audioSource.isPlaying == false && GameManager.Instance.LevelManager._player.Alive)
        {
            _audioSource.PlayOneShot(_flyingSound);
        }
        Touch touch;

        if (Input.touchCount > 0 && (GameManager.Instance.LevelManager.CollectedGreens > 1 || GameManager.Instance.NoFlyConsume))
        {
            touch = Input.GetTouch(0);
            Vector3 v3TouchPos = touch.position;
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId) && _firstTouchOnUI == false ||
                !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                _firstTouchOnUI = false;
                if (_consumeTimer._isRunning == false && !GameManager.Instance.NoFlyConsume)
                {
                    _consumeTimer.StartTimer(_consumeTime);
                }

                _dist = Vector2.Distance(_firstTouchPosition, v3TouchPos);

                if (_dist > _controlsDeadZone)
                {
                    _dist = (_minForce + _maxForce) / 2;
                }

                _dist = Mathf.Clamp(_dist, _minForce, _maxForce);
                _rB.AddForce(transform.up * _dist);
                //_rB.velocity = transform.up * dist;
                _rB.velocity = Vector3.ClampMagnitude(_rB.velocity, _maxSpeed);
            }else
            {
                if (GameManager.Instance.LevelManager._player.Alive)
                {
                    _audioSource.Stop();
                }
                _rB.gravityScale = _gravityScale;
            }
            
        }
        else
        {
            if (GameManager.Instance.LevelManager._player.Alive)
            {
                _audioSource.Stop();
            }
            _rB.gravityScale = _gravityScale;
        }   

    }

    /// <summary>
    /// Turns player character appropriately while flying and changing animation accordingly
    /// </summary>
    private void TurnInFlight() {

        float angle;
        Quaternion angleAxis;

        Touch touch;

        if (Input.touchCount > 0 && (GameManager.Instance.LevelManager.CollectedGreens > 1 || GameManager.Instance.NoFlyConsume))
        {
            touch = Input.GetTouch(0);
            Vector3 v3TouchPos = touch.position;

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId) && _firstTouchOnUI == false ||
                !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                _firstTouchOnUI = false;
                _dist = Vector2.Distance(_firstTouchPosition, v3TouchPos);

                if (_dist > _controlsDeadZone)
                {
                    _dir = v3TouchPos - _firstTouchPosition;
                    angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg - 90;
                    angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
                    CheckRotationDir(angleAxis, transform.rotation);
                    float angleDiff = Quaternion.Angle(angleAxis, transform.rotation);
                    if (angleDiff <= 10)
                    {
                        _animator.SetBool("Turn", false);
                        _animator.SetBool("SlighterTurn", false);
                    }
                    else if (angleDiff > 10 && angleDiff < 45)
                    {
                        _animator.SetBool("Turn", false);
                        _animator.SetBool("SlighterTurn", true);
                    }
                    else if (angleDiff > 45)
                    {
                        _animator.SetBool("Turn", true);
                        _animator.SetBool("SlighterTurn", false);
                    }

                    if (_dist > _minForce)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * _turnSpeed);
                        AimTheArrow(_dist);
                    }
                }
            }else
            {
                _aimArrow.gameObject.SetActive(false);
                _animator.SetBool("Turn", false);
                _animator.SetBool("SlighterTurn", false);
                angle = Mathf.Atan2(_rB.velocity.y, _rB.velocity.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            
        }
        else
        {
            _aimArrow.gameObject.SetActive(false);
            _animator.SetBool("Turn", false);
            _animator.SetBool("SlighterTurn", false);
            angle = Mathf.Atan2(_rB.velocity.y, _rB.velocity.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
    }

    /// <summary>
    /// Aims arrow that shows flying direction
    /// </summary>
    /// <param name="dist"></param>
    private void AimTheArrow(float dist)
    {
        _aimArrow.gameObject.SetActive(true);
        float angle = Mathf.Atan2(_dir.y, _dir.x) * 180 / Mathf.PI - 90;
        _aimArrow.Aim(angle, dist, _maxForce);
    }

    //used to determine if sprite is flipped
    private void CheckRotationDir(Quaternion desiredRot, Quaternion currentRot)
    {

        // define up axis
        Vector3 upAxis = transform.up;

        // mock rotate the axis with each quaternion
        Vector3 vecA = desiredRot * upAxis;
        Vector3 vecB = currentRot * upAxis;

        // now we need to compute the actual 2D rotation projections on the base plane
        float angleA = Mathf.Atan2(vecA.y, vecA.x) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(vecB.y, vecB.x) * Mathf.Rad2Deg;


        // get the signed difference in these angles
        float angleDiff = Mathf.DeltaAngle(angleA, angleB);

        //if bigger than 0 --> clockwise
        if (angleDiff < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    /// <summary>
    /// Determines behaviour depending on what kind of surface/enemy player hits 
    /// </summary>
    /// <param name="collision">Collider of the object player collides with</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == 9 && GameManager.Instance.LevelManager._player.Alive)
        {
            ReduceGreen(_killingFee);
            _greenChange.gameObject.SetActive(false);
            _greenChange.text = (-_killingFee).ToString();
            _greenChange.color = GameManager.Instance.LevelManager._player._red;
            _greenChange.gameObject.SetActive(true);
        }
        else if (_flying && (collision.gameObject.layer == 10 || collision.gameObject.layer == 17) && !collision.gameObject.CompareTag("Rope") && _avoidGroundCollision == false && collision.enabled && !collision.gameObject.CompareTag("Bouncy"))
        {
            _consumeTimer.StopTimer();
            _avoidGroundCollision = true;
            RotateCrash(collision);
            _flying = false;
            _animator.SetBool("Crash", true);
            _animator.SetBool("Fly", false);
            _animator.SetBool("Turn", false);
            _animator.SetBool("SlighterTurn", false);
            _animator.SetBool("Hit", false);
            _rB.velocity = Vector2.zero;
            _rB.gravityScale = 0;
            _aimArrow.gameObject.SetActive(false);
            CrashTax();
        }
        else if (collision.gameObject.layer == 10 && !collision.gameObject.CompareTag("Rope") && _avoidGroundCollision == false && collision.enabled && !collision.gameObject.CompareTag("Bouncy") && _animator.GetBool("TakeOff") == false)
        {
            _consumeTimer.StopTimer();
            _flying = false;
            _animator.SetBool("Crash", false);
            _animator.SetBool("Fly", false);
            _animator.SetBool("Turn", false);
            _animator.SetBool("Throw", false);
            _animator.SetBool("SlighterTurn", false);
            _aimArrow.gameObject.SetActive(false);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_flying && collision.gameObject.layer == 10 && collision.gameObject.CompareTag("Bouncy") )
        {
            _consumeTimer.StopTimer();
            RotateCrash(collision);
            _flying = false;
            _avoidGroundCollision = true;
            _animator.SetBool("Crash", true);
            _animator.SetBool("Fly", false);
            _animator.SetBool("Turn", false);
            _animator.SetBool("Throw", false);
            _animator.SetBool("SlighterTurn", false);
            _animator.SetBool("Hit", false);
            _aimArrow.gameObject.SetActive(false);
            _rB.gravityScale = _gravityScale;
            CrashTax();
        }
    }

    /// <summary>
    /// Consumes some currency when crashing into walls while flying
    /// </summary>
    private void CrashTax()
    {
        if (GameManager.Instance.LevelManager.CollectedGreens > 2 && !GameManager.Instance.LessImpactDamage)
        {
            ReduceGreen(2);
            _greenChange.gameObject.SetActive(false);
            _greenChange.color = GameManager.Instance.LevelManager._player._red;
            _greenChange.text = (-2).ToString();
            _greenChange.gameObject.SetActive(true);
        }
        else if (GameManager.Instance.LevelManager.CollectedGreens == 2 || 
            GameManager.Instance.LessImpactDamage && GameManager.Instance.LevelManager.CollectedGreens > 1)
        {
            ReduceGreen(1);
            _greenChange.gameObject.SetActive(false);
            _greenChange.color = GameManager.Instance.LevelManager._player._red;
            _greenChange.text = (-1).ToString();
            _greenChange.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Prevents player from turning down when taking off to fly
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_flying == false && collision.gameObject.layer == 10 && collision.GetContact(0).normal.y > 0.7f)
        {
            _limitTakeoffRotation = true;
        }
    }

    /// <summary>
    /// Prevents crash from triggering multiple times, if player hits multiple tiles on tilemap
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10) 
        {
            if (_avoidGroundCollision)
            {
                _avoidGroundCollision = false;
            }
            if (_ignoreOneSidedPlatform)
            {
                _ignoreOneSidedPlatform = false;
            }
            if (collision.gameObject.CompareTag("Bouncy"))
            {
                _limitTakeoffRotation = false;
            }
        }
    }

    /// <summary>
    /// Rotates character to align with with wall so crash animation looks better
    /// </summary>
    /// <param name="collision">Collider of the wall that was crashed in to</param>
    private void RotateCrash(Collision2D collision)
    {
        if (GameManager.Instance.LevelManager._player.Alive)
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_crashSound);
        }
        if (Mathf.Abs(collision.GetContact(0).normal.y) < Mathf.Abs(collision.GetContact(0).normal.x))
        {
            if (collision.GetContact(0).normal.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }

        }
        else if (Mathf.Abs(collision.GetContact(0).normal.x) < Mathf.Abs(collision.GetContact(0).normal.y))
        {

            if (collision.GetContact(0).normal.y > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    /// <summary>
    /// Bounces player character away from enemies that do not die in one hit, so player wont just stay on the enemy and take/give more hits
    /// </summary>
    /// <param name="enemy">Transform of the enemy that was hit</param>
    public void BounceFormEnemy(Transform enemy)
    {
        _bounceTimer.StartTimer(1);
        _aimArrow.gameObject.SetActive(false);
        _flying = false;
        _consumeTimer.StopTimer();
        _animator.SetBool("Throw", true);
        _animator.SetBool("Fly", false);
        _animator.SetBool("Turn", false);
        _animator.SetBool("SlighterTurn", false);
        _animator.SetBool("Hit", false);
        Vector2 throwVelocity;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _rB.gravityScale = _gravityScale;
        _rB.velocity = Vector2.zero;
        if (enemy.position.x < transform.position.x)
        {
            throwVelocity.x = 1;
        }
        else
        {
            throwVelocity.x = -1;
        }
        if (enemy.position.y < transform.position.y)
        {
            throwVelocity.y = 1;
        }
        else
        {
            throwVelocity.y = -1;
        }
        _rB.AddForce(throwVelocity * 200);
    }

    /// <summary>
    /// Turns player upright, resets gravity scale and triggers reforming animation
    /// </summary>
    public void ReForm()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _rB.gravityScale = _gravityScale;
        _animator.SetBool("Crash", false);
        _avoidGroundCollision = false;
        _canTakeOff = true;
    }

    /// <summary>
    /// When collectables are consumed, shows a small number to indicate change of value
    /// </summary>
    private void ConsumeGreen()
    {
        if (GameManager.Instance.LevelManager.CollectedGreens > 1)
        {
            GameManager.Instance.LevelManager.ChangeGreenAmount(-_greenConsumeAmount);
            _greenChange.gameObject.SetActive(false);
            _greenChange.color = GameManager.Instance.LevelManager._player._red;
            _greenChange.text = (-_greenConsumeAmount).ToString();
            _greenChange.gameObject.SetActive(true);
        }
    }

    public override void Die()
    {
        base.Die();
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetTrigger("Die");
        }
    }

    private void TimeThrow()
    {
        _animator.SetBool("Throw", false);
    }
}

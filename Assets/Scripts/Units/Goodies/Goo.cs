using UnityEngine;
using UnityEngine.EventSystems;
public class Goo : FriendlyUnitBase
{
    public float _maxVertForce = 200.0f, _maxHorForce = 100.0f, _wallDrag = 10, _normalDrag = 0;
    public int _maxJumpAmount = 2;
    public int _jumpAmount;
    public Vector2 _horBoxCastSize, _downBoxCastSize, _upBoxCastSize;
    private Vector3 _dir;
    public Vector3 _vertUpBoxCastOffset, _vertDownBoxCastOffset, _horLeftBoxCastOffset, _horRightBoxCastOffset;
    private Vector3 _firstTouchPosition;
    private bool _doJump, _ckeckFlip;
    private int _jumpLayerMask = 1 << 10 | 1 << 19 | 1 << 20;
    private AimArrow _aimArrow;
    [HideInInspector]
    public bool _inWater, _justHung, _dontHangfromOneSided, _inWind;
    public AudioClip _jumpSound;
    private CrumblingRock _crumbler;
    private int _stupidFrameCounter = 0;
    private bool _ignoreTouching;

    public Vector3 DIR
    {
        get { return _dir; }
        private set { _dir = value; }
    }

    private void OnEnable()
    {
        _stupidFrameCounter = 0;
    }

    private void OnDisable()
    {
        _animator.SetBool("Landing", false);
        _animator.SetBool("Jumping", false);
        _animator.SetBool("Aiming", false);
        _animator.SetBool("Sliding", false);
        _animator.SetBool("Hanging", false);
        _jumpAmount = _maxJumpAmount;
        _dir = Vector3.zero;
        _doJump = false;
        _aimArrow.gameObject.SetActive(false);
        if (gameObject.GetComponentInChildren<AfterBurn>() != null) {
            gameObject.GetComponentInChildren<AfterBurn>().gameObject.SetActive(false);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _rB = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _aimArrow = gameObject.GetComponentInChildren<AimArrow>();
        _aimArrow.gameObject.SetActive(false);
        _gravityScale = _rB.gravityScale;
        if (GameManager.Instance.TripleJump)
        {
            _maxJumpAmount = 3;
        }
        else
        {
            _maxJumpAmount = 2;
        }
    }

    /// <summary>
    /// Detects touch and jumps accordingly
    /// </summary>
    protected override void Update()
    {
        base.Update();
        if (GameManager.Instance._paused == false && GameManager.Instance.LevelManager._player.Alive == true)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (_jumpAmount > 0)
                        {
                            _firstTouchPosition = touch.position;
                            _ignoreTouching = true;
                            if (_inWater == false)
                            {
                                _animator.SetBool("Aiming", true);
                            }
                        }
                        break;

                    case TouchPhase.Moved:
                        if (_jumpAmount > 0)
                        {
                            CheckSpriteFlip(_firstTouchPosition, false);

                            if (_jumpAmount > 0)
                            {
                                Vector3 currentTouchPos;
                                currentTouchPos = touch.position;
                                float dist = Vector2.Distance(_firstTouchPosition, currentTouchPos);
                                if (dist > _controlsDeadZone)
                                {
                                    _dir = currentTouchPos - _firstTouchPosition;
                                    AimTheArrow(dist);
                                    _dir.x = Mathf.Clamp(_dir.x, -_maxHorForce, _maxHorForce);
                                    _dir.y = Mathf.Clamp(_dir.y, -_maxVertForce, _maxVertForce);
                                    _ignoreTouching = false;
                                }
                            }
                        }
                        break;

                    case TouchPhase.Ended:
                        _aimArrow.gameObject.SetActive(false);

                        if (_jumpAmount > 0 && _ignoreTouching == false)
                        {
                            _doJump = true;
                        }
                        else
                        {
                            _animator.SetBool("Aiming", false);
                        }
                        break;

                    case TouchPhase.Canceled:
                        _aimArrow.gameObject.SetActive(false);

                        if (_jumpAmount > 0 && _ignoreTouching == false)
                        {
                            _doJump = true;
                        }
                        else
                        {
                            _animator.SetBool("Aiming", false);
                        }
                        break;
                }
            }
        }
        
    }

    /// <summary>
    /// Aims arrow that shows player direction of jump
    /// </summary>
    /// <param name="dist"></param>
    private void AimTheArrow(float dist)
    {
        _aimArrow.gameObject.SetActive(true);
        float angle = Mathf.Atan2(_dir.y, _dir.x) * 180 / Mathf.PI - 90;
        _aimArrow.Aim(angle, dist, _maxVertForce);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance._paused == false && GameManager.Instance.LevelManager._player.Alive)
        {
            _animator.ResetTrigger("WaterJump");

            if (_rB.velocity.y > _maxVelocity)
            {
                _rB.velocity = Vector3.ClampMagnitude(_rB.velocity, _maxBounceVelocity);
            }
            else
            {
                _rB.velocity = Vector3.ClampMagnitude(_rB.velocity, _maxVelocity);
            }

            if (_doJump && _animator.GetBool("Hanging") == false)
            {
                Jump();
            }
        }
        //hack to get rid player getting stuck to tilemap corners
        if (_animator.GetBool("Sliding") == true && _rB.velocity.y == 0)
        {
            transform.position = transform.position - new Vector3(0, 0.01f, 0);
        }
        
    }

    private void LateUpdate()
    {
        if (GameManager.Instance._paused == false && GameManager.Instance.LevelManager._player.Alive)
        {
            if (_ckeckFlip)
            {
                CheckSpriteFlip(_firstTouchPosition, false);
            }
            DetectWallsAndGround();
            SpriteChanger();
        }
    }

    /// <summary>
    /// Adds force to jump, counts how many jumps have been made and changes animation accordingly
    /// </summary>
    private void Jump()
    {
        _justHung = false;
        _animator.ResetTrigger("Land");
        _doJump = false;
        _rB.velocity = Vector2.zero;
        _rB.AddForce(_dir);
        _audioSource.PlayOneShot(_jumpSound);
        if (_inWater)
        {
            _animator.ResetTrigger("Flippin");
            _animator.ResetTrigger("Land");
            _animator.SetBool("Aiming", false);
            _animator.SetBool("Jumping", false);
            _animator.SetBool("Airing", false);
            _animator.SetBool("Sliding", false);
            _animator.SetTrigger("WaterJump");
            if (_dir.y > 0)
            {
                _animator.SetBool("Jumping", true);

            }
            else
            {
                _animator.SetBool("Falling", true);

            }
        }
        else
        {
            if (_jumpAmount < _maxJumpAmount)
                _animator.SetTrigger("Flippin");
            _jumpAmount--;
            _animator.SetBool("Jumping", true);
            _animator.SetBool("Aiming", false);
            _animator.SetBool("Falling", false);
            _animator.SetBool("Airing", false);
            _rB.drag = _normalDrag;
        }
    }

    /// <summary>
    /// Prevents wrong animations from triggering when going through one sided platforms. Gets parameters from DetectWallsAndGroudn method below
    /// </summary>
    private void CheckForOneSidedPlatform(RaycastHit2D left, RaycastHit2D right, RaycastHit2D up, RaycastHit2D down)
    {
        if (left.collider != null && left.collider.gameObject.CompareTag("OneSided") && left.collider.transform.up.x < -0.9f) 
        {
            _ignoreOneSidedPlatform = true;
        } 
        else if (right.collider != null && right.collider.gameObject.CompareTag("OneSided") && right.collider.transform.up.x > 0.9f) 
        {
            _ignoreOneSidedPlatform = true;
        } 
        else if (up.collider != null && up.collider.gameObject.CompareTag("OneSided") && up.collider.transform.up.y > 0.9f) 
        { 
            _ignoreOneSidedPlatform = true;
        } 
        else if (down.collider != null && down.collider.gameObject.CompareTag("OneSided") && down.collider.transform.up.y < -0.9f) 
        {
            _ignoreOneSidedPlatform = true;
            _dontHangfromOneSided = true;
        }
        else
        {
            _ignoreOneSidedPlatform = false;
        }

        if(_ignoreOneSidedPlatform && _rB.velocity.magnitude <= 0.01f)
        {
            if (_animator.GetBool("Falling") == true || _animator.GetBool("Airing") == true || _animator.GetBool("Jumping") == true)
            {
                if (down.collider != null || left.collider != null || right.collider != null)
                {
                    _animator.SetTrigger("Land");
                    _animator.SetBool("Jumping", false);
                    _animator.SetBool("Falling", false);
                    _animator.SetBool("Airing", false);
                    _animator.SetBool("Hanging", false);
                    _jumpAmount = _maxJumpAmount;
                }
            }
            
        }
        
    }

    /// <summary>
    /// Detects walls and ground with raycasts to see which animation and physics settings are appropiate. 
    /// </summary>
    private void DetectWallsAndGround()
    {
        RaycastHit2D hitLeft = Physics2D.BoxCast(transform.position + _horLeftBoxCastOffset, _horBoxCastSize, 0, -transform.right, 0, _jumpLayerMask);
        RaycastHit2D hitRight = Physics2D.BoxCast(transform.position + _horRightBoxCastOffset , _horBoxCastSize, 0, transform.right, 0, _jumpLayerMask);
        RaycastHit2D hitUp = Physics2D.BoxCast(transform.position + _vertUpBoxCastOffset, _upBoxCastSize,0, transform.up, 0, _jumpLayerMask);
        RaycastHit2D hitDown = Physics2D.BoxCast(transform.position + _vertDownBoxCastOffset, _downBoxCastSize, 0, -transform.up, 0, _jumpLayerMask);

        CheckForOneSidedPlatform(hitLeft, hitRight, hitUp, hitDown);


        if (_ignoreOneSidedPlatform == false && _inWater == false)
        {
            if(hitLeft.collider == null && hitRight.collider == null)
            {
                _animator.SetBool("Sliding", false);
            }

            if (hitDown.collider != null)
            {
                _animator.ResetTrigger("Flippin");

                if (_rB.velocity.y == 0 && _animator.GetBool("Sliding") == true)
                {
                    _stupidFrameCounter++;
                    if (_stupidFrameCounter >= 5)
                    {
                        _animator.SetBool("Sliding", false);
                        _stupidFrameCounter = 0;
                    }
                }
                else
                {
                    _stupidFrameCounter = 0;
                }
                _rB.drag = _normalDrag;
                if (_animator.GetBool("Falling") == true || _animator.GetBool("Airing") == true)
                {
                    _animator.SetTrigger("Land");
                    if (hitDown.collider.gameObject.CompareTag("Bouncy"))
                    {
                        if (_rB.velocity.y == 0)
                        {
                            _animator.ResetTrigger("Land");
                        }
                        else
                        {
                            _animator.SetBool("Jumping", true);
                            _jumpAmount = _maxJumpAmount - 1;
                        }
                    }
                    else
                    {
                        _animator.SetBool("Jumping", false);
                    }
                    _animator.SetBool("Falling", false);
                    _animator.SetBool("Airing", false);
                    _animator.SetBool("Hanging", false);
                }
            }
            else if (hitUp.collider != null)
            {
                if (!hitUp.collider.gameObject.CompareTag("Bouncy") && !hitUp.collider.gameObject.CompareTag("Slippery") &&
                    !hitUp.collider.gameObject.CompareTag("SpinningPlant") && !hitUp.collider.gameObject.CompareTag("CannonBall") &&
                    hitUp.collider.gameObject.layer != 20 && _inWind == false)
                {
                    if (hitUp.collider.gameObject.CompareTag("MovingPlatform") && _justHung == false || hitUp.collider.gameObject.CompareTag("Cannon") && _justHung == false || hitUp.collider.gameObject.CompareTag("CannonBarrel") && _justHung == false || 
                        !hitUp.collider.gameObject.CompareTag("MovingPlatform") && !hitUp.collider.gameObject.CompareTag("CannonBarrel") && !hitUp.collider.gameObject.CompareTag("Cannon"))
                    {
                        if (hitUp.collider.gameObject.CompareTag("OneSided") && _dontHangfromOneSided == false || !hitUp.collider.gameObject.CompareTag("OneSided"))
                        {
                            if (hitUp.collider.gameObject.CompareTag("Crumbling") && _crumbler == null)
                            {
                                _crumbler = hitUp.collider.gameObject.GetComponent<CrumblingRock>();
                                _crumbler.AlmostCrumble();
                            }
                            _animator.SetBool("Hanging", true);
                            _rB.drag = _wallDrag * 10;
                            _animator.SetBool("Sliding", false);
                            _animator.SetBool("Jumping", false);
                            _animator.SetBool("Falling", false);
                            _animator.SetBool("Airing", false);
                        }
                    }
                }
            }
            else if (hitLeft.collider != null && hitRight.collider == null)
            {
                
                if (hitLeft.collider.gameObject.CompareTag("Bouncy"))
                {
                    _animator.SetBool("Jumping", true);
                    _jumpAmount = _maxJumpAmount - 1;
                }
                else
                {
                    if (hitLeft.collider.gameObject.CompareTag("Slippery"))
                    {
                        _rB.drag = _normalDrag;
                    }
                    else
                    {
                        _rB.drag = _wallDrag;
                    }

                    _animator.SetBool("Sliding", true);
                    _animator.SetBool("Jumping", false);
                    _animator.SetBool("Falling", false);
                    _animator.SetBool("Airing", false);
                    _animator.SetBool("Hanging", false);
                    _animator.SetBool("Aiming", false);
                    _spriteRenderer.flipX = true;
                }

                if (hitLeft.collider.gameObject.CompareTag("Crumbling"))
                {
                    if (_crumbler != null && hitLeft.collider.gameObject != _crumbler.gameObject)
                    {
                        _crumbler.Crumble();
                        _crumbler = hitLeft.collider.gameObject.GetComponent<CrumblingRock>();
                        _crumbler.AlmostCrumble();

                    }
                    else if (_crumbler == null)
                    {
                        _crumbler = hitLeft.collider.gameObject.GetComponent<CrumblingRock>();
                        _crumbler.AlmostCrumble();
                    }
                }
                else if (_crumbler != null && !hitLeft.collider.gameObject.CompareTag("Crumbling"))
                {
                    _crumbler.Crumble();
                    _crumbler = null;
                }

            }
            else if (hitRight.collider != null && hitLeft.collider == null)
            {
                
                if (hitRight.collider.gameObject.CompareTag("Bouncy"))
                {
                    _animator.SetBool("Jumping", true);
                    _jumpAmount = _maxJumpAmount - 1;
                }
                else
                {
                    if (hitRight.collider.gameObject.CompareTag("Slippery"))
                    {
                        _rB.drag = _normalDrag;
                    }
                    else
                    {
                        _rB.drag = _wallDrag;
                    }
                    _animator.SetBool("Sliding", true);
                    _animator.SetBool("Jumping", false);
                    _animator.SetBool("Falling", false);
                    _animator.SetBool("Airing", false);
                    _animator.SetBool("Hanging", false);
                    _animator.SetBool("Aiming", false);
                    _spriteRenderer.flipX = false;
                }

                if (hitRight.collider.gameObject.CompareTag("Crumbling"))
                {
                    if (_crumbler != null && hitRight.collider.gameObject != _crumbler.gameObject)
                    {
                        _crumbler.Crumble();
                        _crumbler = hitRight.collider.gameObject.GetComponent<CrumblingRock>();
                        _crumbler.AlmostCrumble();

                    }
                    else if (_crumbler == null)
                    {
                        _crumbler = hitRight.collider.gameObject.GetComponent<CrumblingRock>();
                        _crumbler.AlmostCrumble();
                    }
                }
                else if(_crumbler != null && !hitRight.collider.gameObject.CompareTag("Crumbling"))
                {
                    _crumbler.Crumble();
                    _crumbler = null;
                }
                
            }
            else
            {
                if(_crumbler != null)
                {
                    _crumbler.Crumble();
                    _crumbler = null; 
                }
                _rB.drag = _normalDrag;
                _animator.SetBool("Sliding", false);
                _ckeckFlip = true;
                _animator.ResetTrigger("Land");
                _animator.SetBool("Hanging", false);
            }

            if (hitLeft.collider != null || hitRight.collider != null || hitDown.collider != null)
            {
                if (_animator.GetBool("Jumping") == false)
                    _jumpAmount = _maxJumpAmount;
            }else if (hitLeft.collider == null && hitRight.collider == null && hitDown.collider == null && _jumpAmount == _maxJumpAmount && _animator.GetBool("Falling") == true)
            {
                _jumpAmount = _maxJumpAmount - 1;
            }

            if (hitLeft.collider == null && hitRight.collider == null && hitDown.collider == null && hitUp.collider == null)
            {
                _dontHangfromOneSided = false;
            }
        }
        else
        {
            _rB.drag = _normalDrag;
            _ckeckFlip = true;
        }

    }

    /// <summary>
    /// flip sprite on X axis
    /// </summary>
    /// <param name="position">first touch position or first position of click</param>
    /// <param name="boolean">true if sliding on wall is allowed or false if sliding is not allowed</param>
    private void CheckSpriteFlip(Vector3 position, bool boolean)
    {
        if (boolean == false)
        {
            if (position.x > Input.mousePosition.x && _animator.GetBool("Sliding") == false)
            {
                _spriteRenderer.flipX = true;
                _ckeckFlip = false;
            }
            else if (position.x < Input.mousePosition.x && _animator.GetBool("Sliding") == false)
            {
                _spriteRenderer.flipX = false;
                _ckeckFlip = false;
            }
        }
    }
    
    /// <summary>
    /// Changes animation mid jump to give more life to the character
    /// </summary>
    private void SpriteChanger()
    {
        if (_rB.velocity.y > -2f && _rB.velocity.y < 2f && _animator.GetBool("Sliding") == false && _animator.GetBool("Jumping") == true)
        {
            _animator.SetBool("Airing", true);
            _animator.SetBool("Jumping", false);
            _animator.SetBool("Falling", false);
        }
        else if (_rB.velocity.y < -2f && _animator.GetBool("Sliding") == false)
        {
            _animator.SetBool("Falling", true);
            _animator.SetBool("Jumping", false);
            _animator.SetBool("Airing", false);
        }

    }

    public override void Die()
    {
        base.Die();
        {
            _animator.SetTrigger("Die");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("SpinningPlant"))
        {
            _inWind = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SpinningPlant"))
        {
            _inWind = false;
        }
    }
}
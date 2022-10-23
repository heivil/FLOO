using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : BadGuyUnitBase
{
    public float _fwCastLength = 0.5f, _dwnCastLengt = 1.0f, _speed = 1;
    private int _groundLayerMask = 1 << 10;
    private bool _rotateRight = false, _rotateLeft = false, _addDownForce = true;
    [HideInInspector]
    public Timer _turnTimer;
    private Quaternion _startRotation;
    private SpriteRenderer _renderer;
    private Vector3 _direct;
    public GameObject _fallDecider;
    private Vector3 _fdNegScale = new Vector3(-1,1,1), _fdPosScale = new Vector3(1, 1, 1);

    public enum Direction
    {
        Right = 0,
        Left = 1
    }
    public Direction _dir;

    protected override void Awake()
    {
        base.Awake();
        _rB = GetComponent<Rigidbody2D>();
        _turnTimer = gameObject.AddComponent<Timer>();
        _startRotation = transform.rotation;
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _turnTimer.OnTimerCompleted += DoneTurning;
    }

    private void OnDestroy()
    {
        _turnTimer.OnTimerCompleted -= DoneTurning;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!_rotateRight && !_rotateLeft && _rB.gravityScale == 0 && _addDownForce)
        {
            _rB.velocity = -transform.up;
        }else if (_rB.gravityScale == 0 && !_addDownForce)
        {
            _rB.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// Moves and rotates unit according to situation
    /// </summary>
    void Update()
    {
        if(_dir == Direction.Right)
        {
            _renderer.flipX = true;
            _fallDecider.transform.localScale = _fdNegScale;
        }
        else if(_dir == Direction.Left)
        {       
            _renderer.flipX = false;
            _fallDecider.transform.localScale = _fdPosScale;
        }
        
        if(_rotateRight)
        {
            if(_dir == Direction.Right)
            {
                RotateRight();
            }else if (_dir == Direction.Left)
            {
                RotateRight();
                transform.Translate(-Vector3.right * Time.deltaTime * 0.75f);
            }
            
        }else if (_rotateLeft)
        {
            if(_dir == Direction.Right)
            {
                RotateLeft();
                transform.Translate(Vector3.right * Time.deltaTime * 0.75f);
            }else if (_dir == Direction.Left)
            {
                RotateLeft();
            }
            
        }
        else
        {
            if(_dir == Direction.Right) 
            {
                transform.Translate(Vector3.right * Time.deltaTime * _speed);
                CastRays(Vector3.right);
            }else if (_dir == Direction.Left)
            {
                transform.Translate(-Vector3.right * Time.deltaTime * _speed);
                CastRays(-Vector3.right);
            }
            
        }
    }

    /// <summary>
    /// Detects corners and ground to see if unit should move forwards or rotate according to corners. 
    /// Also adds force downwards (local space) to keep unit attached to ground even if upside down or sideways
    /// </summary>
    /// <param name="dir"></param>
    private void CastRays(Vector3 dir)
    {
        _direct = dir;
        Vector2 forwardBoxSize = new Vector2(_fwCastLength, 0.2f);

        if(transform.up.y < 0.1f && transform.up.y > -0.1f)
        {
            forwardBoxSize = new Vector2(0.2f, _fwCastLength);
        }
        //RaycastHit2D forwardHit = Physics2D.Raycast(transform.position + transform.up / 2, transform.TransformDirection(dir), _fwCastLength, _groundLayerMask);
        RaycastHit2D forwardHit = Physics2D.BoxCast(transform.position + transform.up / 3, forwardBoxSize, 0, transform.TransformDirection(dir), 0.25f, _groundLayerMask);

        if (forwardHit.collider == null || 
            _dir == Direction.Right && forwardHit.collider.transform.up == transform.right && forwardHit.collider.gameObject.CompareTag("OneSided") || 
            _dir == Direction.Left && forwardHit.collider.transform.up == -transform.right && forwardHit.collider.gameObject.CompareTag("OneSided"))
        {
            RaycastHit2D downHit = Physics2D.Raycast(transform.position, -transform.up, _dwnCastLengt, _groundLayerMask);
            RaycastHit2D secondDownHit = Physics2D.Raycast(transform.position + transform.TransformDirection(dir) * 0.2f, -transform.up, _dwnCastLengt, _groundLayerMask);
            if(downHit.collider != null)
            {
                _addDownForce = true;
            }
            else
            {
                _addDownForce = false;
            }

            if (downHit.collider == null && secondDownHit.collider == null)
            {
                
                if (_dir == Direction.Right)
                {
                    _rotateRight = true;
                }
                else if (_dir == Direction.Left)
                {
                    _rotateLeft = true;
                }

            }
            
        }
        else
        {
            if (_dir == Direction.Right)
            {
                _rotateLeft = true;
            }else if (_dir == Direction.Left)
            {
                _rotateRight = true;
            }
        }
    }

    /// <summary>
    /// Rotates unit at corners and does not add force down while rotating
    /// </summary>
    private void RotateRight()
    {
        _addDownForce = false;
        if (_dir == Direction.Right)
        {
            _animator.SetBool("OuterCorner", true);
        }else if (_dir == Direction.Left)
        {
            _animator.SetBool("InnerCorner", true);
        }
        
        Quaternion ninetyDegrees = Quaternion.Euler(0, 0, -90);
        if (_turnTimer._isRunning == false)
        {
            _startRotation = transform.rotation;
            _turnTimer.StartTimer(1);
        }
        transform.rotation = Quaternion.Slerp(_startRotation, _startRotation * ninetyDegrees, _turnTimer.NormalizedTimeElapsed );
    }

    /// <summary>
    /// Rotates unit at corners and does not add force down while rotating
    /// </summary>
    private void RotateLeft()
    {
        _addDownForce = false;
        if (_dir == Direction.Right)
        {
            _animator.SetBool("InnerCorner", true);
        }
        else if (_dir == Direction.Left)
        {
            _animator.SetBool("OuterCorner", true);
        }
        
        Quaternion ninetyDegrees = Quaternion.Euler(0, 0, 90);
        if (_turnTimer._isRunning == false)
        {
            _startRotation = transform.rotation;
            _turnTimer.StartTimer(1);
        }
        transform.rotation = Quaternion.Slerp(_startRotation, _startRotation * ninetyDegrees, _turnTimer.NormalizedTimeElapsed);
    }

    /// <summary>
    /// After rotating, applies force to stay on walls and ceiling.
    /// </summary>
    private void DoneTurning()
    {
        if (_rotateLeft)
        {
            _rotateLeft = false;
            _animator.SetBool("InnerCorner", false);
            _animator.SetBool("OuterCorner", false);
        }
        else if (_rotateRight)
        {
            _rotateRight = false;
            _animator.SetBool("OuterCorner", false);
            _animator.SetBool("InnerCorner", false);
        }
        _addDownForce = true;
    }

    /// <summary>
    /// Changes moving direction
    /// </summary>
    public void ChangeDir()
    {
        if (_dir == Direction.Right)
        {
            _dir = Direction.Left;
        }
        else if (_dir == Direction.Left)
        {
            _dir = Direction.Right;
        }
    }

}

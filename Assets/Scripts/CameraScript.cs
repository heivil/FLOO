using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraScript : MonoBehaviour
{
    public GameObject _followUnit;
    public float _xMove = 1.5f, _yMove = 2.5f, _lerpTime = 0.5f;
    private bool _isLerpingX, _isLerpingY, _isFollowingX, _isFollowingY;
    public SpriteRenderer _backGround;
    private string _upDown, _leftRight, _previousLeftRigth = "", _previousUpDown = "";
    private Vector2 _previousTargetPosX, _previousTargetPosY;
    private float _currentX, _currentY, _maxX, _maxY;
    private Timer _xLerpTimer, _yLerpTimer;

    private void Awake()
    {
        _xLerpTimer = gameObject.AddComponent<Timer>();
        _yLerpTimer = gameObject.AddComponent<Timer>();
        _xLerpTimer.OnTimerCompleted += CompleteLerpX;
        _yLerpTimer.OnTimerCompleted += CompleteLerpY;
    }
    private void Start()
    {
        float orthoSize = _backGround.bounds.size.x * Screen.height / Screen.width / 2;
        Camera.main.orthographicSize = orthoSize / 2;
        _maxX = _backGround.bounds.size.x / 4;
        _maxY = _backGround.bounds.size.y / 2 - Camera.main.orthographicSize;

        _followUnit = FindObjectOfType<Player>()._activeUnit;
        if (_followUnit == null)
        {
            Debug.LogError("Warning, no player!");
        }
        _previousTargetPosX = _followUnit.transform.position;
    }

    private void OnDestroy()
    {
        _xLerpTimer.OnTimerCompleted -= CompleteLerpX;
        _yLerpTimer.OnTimerCompleted -= CompleteLerpY;
    }

    private void LateUpdate()
    {
        MoveCam();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_maxX, _maxX), Mathf.Clamp(transform.position.y, -_maxY - 2, _maxY + 2), transform.position.z);
    }
    private void MoveCam()
    {

        if (gameObject.transform.position.x > _followUnit.transform.position.x && _isLerpingX == false && _isFollowingX == false)
        {
            float difference = gameObject.transform.position.x - _followUnit.transform.position.x;
            if (difference >= _xMove)
            {
                _isLerpingX = true;
                _xLerpTimer.StartTimer(_lerpTime);
            }
        }
        else if (gameObject.transform.position.x < _followUnit.transform.position.x && _isLerpingX == false && _isFollowingX == false)
        {
            float difference = _followUnit.transform.position.x - gameObject.transform.position.x ;
            if (difference >= _xMove)
            {
                _isLerpingX = true;
                _xLerpTimer.StartTimer(_lerpTime);
            }
        }   

        if (_isLerpingX)
        {
            _currentX = Mathf.Lerp(transform.position.x, _followUnit.transform.position.x, _xLerpTimer.NormalizedTimeElapsed);
        }

        if (gameObject.transform.position.y > _followUnit.transform.position.y && _isLerpingY == false && _isFollowingY == false)
        {
            float difference =  gameObject.transform.position.y - _followUnit.transform.position.y;
            if (difference >= _yMove)
            {
                _isLerpingY = true;
                _yLerpTimer.StartTimer(_lerpTime);
            }
        }
        else if (gameObject.transform.position.y < _followUnit.transform.position.y && _isLerpingY == false && _isFollowingY == false)
        {
            float difference =  _followUnit.transform.position.y - transform.position.y;
            if (difference >= _yMove)
            {
                _isLerpingY = true;
                _yLerpTimer.StartTimer(_lerpTime);
            }
        }

        if (_isLerpingY)
        {
            _currentY = Mathf.Lerp(transform.position.y ,_followUnit.transform.position.y , _yLerpTimer.NormalizedTimeElapsed);
        }

        // original stupidness, here as a reminder if something brakes :)
        /*if((transform.position.x - _followUnit.transform.position.x) < 0.25f && (transform.position.x - _followUnit.transform.position.x) > -0.25f && _isLerpingX == true)
        {
            _isLerpingX = false;     
            _isFollowingX = true;
        }

        if ((transform.position.y - _followUnit.transform.position.y) < 0.25f && (transform.position.y - _followUnit.transform.position.y) > -0.25f && _isLerpingY == true)
        {
            _isLerpingY = false;       
            _isFollowingY = true;
        }*/      

        if(_isFollowingX )
        {
            FollowTargetX();
        }
        if (_isFollowingY)
        {
            FollowTargetY();
        }
        if (_isLerpingX)
        {
            transform.position = new Vector3(_currentX, transform.position.y, -10);
        } 
        if (_isLerpingY)
        {
            transform.position = new Vector3(transform.position.x, _currentY, -10);
        }
        
    }

   private void FollowTargetX()
    {
        Vector2 currentTargetPos = _followUnit.transform.position;       

        if (currentTargetPos.x < _previousTargetPosX.x)
        {
            _leftRight = "Left";
        }
        else if (currentTargetPos.x > _previousTargetPosX.x)
        {
            _leftRight = "Right";
        }
        else
        {
            _leftRight = "Still";
        }

        if(_previousLeftRigth == "")
        {
            _previousLeftRigth = _leftRight;
        }

        if (_leftRight == _previousLeftRigth)
        {
            
            _isFollowingX = true;
            transform.position = new Vector3(_followUnit.transform.position.x, transform.position.y, -10);
        }
        else if(_previousLeftRigth == "Right" && _leftRight == "Left" || _previousLeftRigth == "Left" && _leftRight == "Right")
        {
            _isFollowingX = false;
        }

        _previousTargetPosX = _followUnit.transform.position;

        if(_leftRight != "Still")
        {
            _previousLeftRigth = _leftRight;
        }
   
    }

    private void FollowTargetY()
    {
        Vector2 currentTargetPos = _followUnit.transform.position;

        if (currentTargetPos.y < _previousTargetPosY.y)
        {
            _upDown = "Down";
        }
        else if (currentTargetPos.y > _previousTargetPosY.y)
        {
            _upDown = "Up";
        }
        else
        {
            _upDown = "Still";
        }

        if (_previousUpDown == "")
        {
            _previousUpDown = _upDown;
        }

        if (_upDown == _previousUpDown)
        {
            
            _isFollowingY = true;
            transform.position = new Vector3(transform.position.x, _followUnit.transform.position.y, -10);
        }
        else if(_previousUpDown == "Up" && _upDown == "Down" || _previousUpDown == "Down" && _upDown == "Up")
        {
            _isFollowingY = false;
        }

        _previousTargetPosY = _followUnit.transform.position;

        if (_upDown != "Still")
        {
            _previousUpDown = _upDown;
        }
    }

    private void CompleteLerpX()
    {
        _isLerpingX = false;
        _isFollowingX = true;
    }

    private void CompleteLerpY()
    {
        _isLerpingY = false;
        _isFollowingY = true;
    }
}
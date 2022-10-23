using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] _targets = new Transform[2];
    public bool _moving = false, _adoptObject = true, _looping = false;
    private int _movingInt;
    public float _speed = 1;
    private bool _goingForward;

    private void Awake()
    {
        _movingInt = 0;

    }
    private void Update()
    {
        if (_moving && _looping)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targets[_movingInt].transform.position, _speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _targets[_movingInt].position) < 0.05f)
            {
                if (_movingInt < _targets.Length -1)
                {
                    _movingInt++;
                }
                else
                {
                    _movingInt = 0;
                }
            }
        }
        else if(_moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targets[_movingInt].transform.position, _speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _targets[_movingInt].position) < 0.05f)
            {
                if (_movingInt < _targets.Length -1 && _goingForward)
                {
                    _movingInt++;
                }
                else if (_movingInt > 0 && !_goingForward)
                {
                    _movingInt--;
                }

                if (_movingInt == _targets.Length -1)
                {
                    _goingForward = false;
                }else if (_movingInt == 0)
                {
                    _goingForward = true;
                }
            }
        }
    }

    public void ToggleOnOff(bool move)
    {
        if (move)
        {
            _moving = true;
        }
        else
        {
            _moving = false;
        }
    }
    
    /// <summary>
    /// Make objects with rigidbody chid so they move with platform
    /// </summary>
    /// <param name="collision">Other collider</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody2D>() != null && _adoptObject && collision.gameObject.CompareTag("CannonBarrel") == false)
        {
            if (collision.gameObject.transform.parent != null && collision.gameObject.CompareTag("Flyer") == false)
            {
                collision.gameObject.transform.parent.transform.parent = transform;
            }
            else
            {
                collision.gameObject.transform.parent = transform;
            }
        }
        else if (collision.gameObject.activeInHierarchy && collision.gameObject.GetComponent<Rigidbody2D>() != null && _adoptObject == false)
        {
            if (collision.gameObject.layer == 8)
            {
                collision.gameObject.transform.parent.transform.parent = transform;
            }
        }
    }

    /// <summary>
    /// Unchild objects with rigidbody when they stop touching platform
    /// </summary>
    /// <param name="collision">Other collider</param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.activeInHierarchy && collision.gameObject.GetComponent<Rigidbody2D>() != null && _adoptObject)
        {
            if (collision.gameObject.transform.parent.transform.parent == transform)
            {
                collision.gameObject.transform.parent.transform.parent = null;
            }
            else if(collision.gameObject.transform.parent == transform)
            {
                collision.gameObject.transform.parent = null;
            }
        }
        else if (collision.gameObject.activeInHierarchy && collision.gameObject.GetComponent<Rigidbody2D>() != null && _adoptObject == false)
        {
            if (collision.gameObject.layer == 8)
            {
                collision.gameObject.transform.parent.transform.parent = null;
            }
        }
        
    }

}

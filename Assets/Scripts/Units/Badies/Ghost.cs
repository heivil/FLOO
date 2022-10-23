using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : BadGuyUnitBase, ITargeter
{
    public float _speed = 1;
    private GameObject _target;
    //private SpriteRenderer _renderer;
    public ParticleSystem _fogParticle;
    private Targeter _targeter;
    private Vector2 _moveHere = new Vector2(0,0);
    public Collider2D _collider;

    protected override void Awake()
    {
        base.Awake();
        //_renderer = GetComponentInChildren<SpriteRenderer>();
        _targeter = transform.parent.gameObject.GetComponentInChildren<Targeter>();
        _moveHere = _targeter.GetRandomPoint();
        _collider = gameObject.GetComponent<Collider2D>();
    }

    /// <summary>
    /// Moves between random points in patrol area, or towards attack target
    /// </summary>
    void Update()
    {
        if(_target != null)
        {
            if(_target.transform.position.x < transform.position.x)
            {
                LookAtThis(_target.transform.position);
            }else
            {
                LookAtThis(_target.transform.position);
            }
            if (Vector2.Distance(transform.position, _target.transform.position) > 0.1f) 
            {
                transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
            }
        } else
        {
            
            transform.position = Vector2.MoveTowards(transform.position, _moveHere, _speed * Time.deltaTime);
            
            if (Vector2.Distance(transform.position, _moveHere) < 0.1f)
            {
                _moveHere = _targeter.GetRandomPoint();
            }
            LookAtThis(_moveHere);
    
        }
    }
    
    /// <summary>
    /// Looks in direction where unit is moving
    /// </summary>
    /// <param name="lookHere"></param>
    private void LookAtThis(Vector2 lookHere)
    {
        transform.LookAt(new Vector3(lookHere.x, transform.position.y, transform.position.z));
        transform.rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
    }

    public void Target(GameObject target)
    {
        _target = target;
    }

    public void ClearTarget()
    {
        _target = null;
        _moveHere = _targeter.GetRandomPoint();
    }

    public override void Die()
    {
        _fogParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        base.Die();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Physics2D.IgnoreCollision(collision.collider, _collider);
        }else  {
            base.OnCollisionEnter2D(collision);
        }
    }

    }
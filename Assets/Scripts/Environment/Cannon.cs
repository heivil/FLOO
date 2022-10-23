using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private CannonBall _cannonBall;
    public float _shotVelocity = 5;
    public int _shotAmount = 5;
    private Vector2 _shootDirection;
    public Transform _cannonBallStartPos;
    public CannonBallPool _cannonBallPool;
    private int _shotCounter = 0;
    public Animator _anim;
    public GameObject _barrel;
    private CapsuleCollider2D _thisCollider;
    public AudioSource _audioSource;
    public AudioClip _boomSound;

    private void Awake()
    {
        _barrel.GetComponent<Rigidbody2D>().centerOfMass = new Vector2(0,0);
        _thisCollider = gameObject.GetComponent<CapsuleCollider2D>();
        Physics2D.IgnoreCollision(_barrel.GetComponent<Collider2D>(), _thisCollider);
    }

    /// <summary>
    /// Fires cannonballs to the direction of cannons barrel until there are no shots left
    /// </summary>
    public void FireCannon()
    {
        if (_shotCounter < _shotAmount) {
            _audioSource.PlayOneShot(_boomSound);
            _anim.SetTrigger("Fire");
            _shotCounter++;
            _cannonBall = _cannonBallPool.GetPooledObject();
            _cannonBall.gameObject.SetActive(true);
            _cannonBall.transform.position = _cannonBallStartPos.position;
            _shootDirection = _barrel.transform.up;
            _cannonBall.GetComponent<Rigidbody2D>().velocity = _shootDirection * _shotVelocity;
        }
    }
}

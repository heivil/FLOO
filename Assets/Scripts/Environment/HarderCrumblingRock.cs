using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarderCrumblingRock : MonoBehaviour
{
    private Animator _animator;
    private Collider2D _collider;
    public AudioClip _crumbleSound;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _collider = gameObject.GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CannonBall") && collision.relativeVelocity.magnitude > 12 || collision.gameObject.layer == 20) 
        {
            Crumble();
        }
    }
    public void Crumble()
    {
        _animator.SetTrigger("Crumble");
        _collider.enabled = false;
        GameManager.Instance.AudioManager.PlayASound(_crumbleSound, true, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Explosion"))
        {
            Crumble();
        }
    }
}

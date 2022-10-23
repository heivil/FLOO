using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    
    public Animator _animator;
    public float _minBounce = 2; 

    /// <summary>
    /// Bounces objects with rigidbodies with appropriate force and direction
    /// </summary>
    /// <param name="collision">collider of other gameobject</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            Rigidbody2D RB = collision.gameObject.GetComponent<Rigidbody2D>();
            _animator.SetTrigger("Bounce");
            if (Mathf.Abs(collision.GetContact(0).normal.y) < Mathf.Abs(collision.GetContact(0).normal.x))
            {
                RB.velocity = collision.relativeVelocity * new Vector2(-1.5f, 1);
                if (collision.gameObject.layer == 8)
                {
                    if (RB.velocity.x > 0 && RB.velocity.x < _minBounce) 
                    {
                        RB.velocity = new Vector2(_minBounce, RB.velocity.y);
                    }else if (RB.velocity.x < 0 && RB.velocity.x > -_minBounce)
                    {
                        RB.velocity = new Vector2(-_minBounce, RB.velocity.y);
                    }
                }
            }
            else if (Mathf.Abs(collision.GetContact(0).normal.x) < Mathf.Abs(collision.GetContact(0).normal.y))
            {
                RB.velocity = collision.relativeVelocity * new Vector2(1, -1.5f);
                if (collision.gameObject.layer == 8)
                {
                    if (RB.velocity.y >= 0 && RB.velocity.y < _minBounce)
                    {
                        
                        RB.velocity = new Vector2(RB.velocity.x, _minBounce);
                    }
                    else if (RB.velocity.y < 0 && RB.velocity.y > -_minBounce)
                    {
                        RB.velocity = new Vector2(RB.velocity.x, -_minBounce);
                    }
                }
            }
        }
    }
}

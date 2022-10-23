using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField]
    private float _killSpeed = 5;
    private Rigidbody2D _rB;
    public float _maxVelocity = 20;
    public bool _kill;
    private int _damage;
    private void Awake()
    {
        _rB = gameObject.GetComponent<Rigidbody2D>();
        if (GameManager.Instance.LessDamage)
        {
            _damage = 8;
        }else
        {
            _damage = 10;
        }
    }
    private void FixedUpdate()
    {
        _rB.velocity = Vector3.ClampMagnitude(_rB.velocity, _maxVelocity);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > _killSpeed)
        {
            if (collision.gameObject.CompareTag("Rope"))
            {
                collision.gameObject.GetComponent<Rope>().Snap();
            }
            else if (collision.gameObject.layer == 8 && _kill)
            {
                collision.gameObject.GetComponentInChildren<PlayerHit>().TakeTheHit(_damage, false);
            }
            else if (collision.gameObject.layer == 9 && _kill)
            {
                collision.gameObject.GetComponent<BadGuyUnitBase>().Die();
            }
            /*else if (collision.gameObject.CompareTag("AcidContainer"))
            {
                collision.gameObject.GetComponent<AcidContainer>().CrackOpen();
            }*/
        }
    }
}

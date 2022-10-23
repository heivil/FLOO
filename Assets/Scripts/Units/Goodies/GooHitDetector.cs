using UnityEngine;

public class GooHitDetector : PlayerHit
{
    private Goo _goo;

    protected override void Awake()
    {
        base.Awake();
        _goo = GetComponentInParent<Goo>();
        _renderer = _goo._spriteRenderer;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        
        if (collision.gameObject.layer == 9 && _immortalityTimer._isRunning == false)
        {
            if (collision.transform.position.x < transform.position.x)
            {
                _pushDirection = new Vector2(1, 1);
            }
            else
            {
                _pushDirection = new Vector2(-1, 1);
            }
            Physics2D.IgnoreLayerCollision(8, 9, true);
            TakeTheHit(_damageAmount, true);
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
        if (collision.gameObject.layer == 17)
        {
            _goo._jumpAmount = _goo._maxJumpAmount;
        }
        if (collision.gameObject.layer == 9 && _immortalityTimer._isRunning == false)
        {
            if (collision.transform.position.x < transform.position.x)
            {
                _pushDirection = new Vector2(1, 1);
            }
            else
            {
                _pushDirection = new Vector2(-1, 1);
            }
            Physics2D.IgnoreLayerCollision(8, 9, true);
            TakeTheHit(_damageAmount, true);
        }
    }

    public override void TakeTheHit(int damage, bool pushBack)
    {
        base.TakeTheHit(damage, pushBack);
        if (GameManager.Instance.LevelManager._player.Alive && GameManager.Instance.LevelManager._levelEnded == false)
        {
            _goo._rB.drag = _goo._normalDrag;
            _animator.SetBool("Hit", true);
        }
    }

    public override void TakeTheHit(int damage, bool pushBack, bool ignore)
    {
        base.TakeTheHit(damage, pushBack, ignore);
        if (GameManager.Instance.LevelManager._player.Alive)
        {
            _goo._rB.drag = _goo._normalDrag;
            _animator.SetBool("Hit", true);
        }
    }
}

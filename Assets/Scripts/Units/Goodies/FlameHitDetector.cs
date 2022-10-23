using UnityEngine;

public class FlameHitDetector : PlayerHit
{
    private Flame _flame;
    
    protected override void Awake()
    {
        base.Awake();
        _flame = GetComponentInParent<Flame>();
        _renderer = _flame._spriteRenderer;
    }

    public override void TakeTheHit(int damage, bool pushBack)
    {
        
        if (pushBack && _immortalityTimer._isRunning == false && GameManager.Instance.LevelManager._levelEnded == false)
        {
            _flame._flying = false;
            _animator.SetBool("Fly", false);
            _animator.SetBool("Turn", false);
            _animator.SetBool("SlighterTurn", false);
            _animator.SetBool("TakeOff", false);
        }

        if (GameManager.Instance.LevelManager._player.Alive && _animator.GetBool("Crash") == false && _immortalityTimer._isRunning == false && GameManager.Instance.LevelManager._levelEnded == false)
        {
            _animator.SetBool("Hit", true);
        }
        base.TakeTheHit(damage, pushBack);
        _flame._consumeTimer.StopTimer();
    }
}

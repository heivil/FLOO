using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingRock : MonoBehaviour
{
    private Timer _respawnTimer;
    public float _respawnTime = 5.0f;
    private Collider2D _collider;
    public Animator _animator;
    public bool _doRespawn = false;
    public AudioClip _crumbleSound;
    private void Awake()
    {
        _respawnTimer = gameObject.AddComponent<Timer>();
        _respawnTimer.OnTimerCompleted += Respawn;
        _collider = gameObject.GetComponent<Collider2D>();
        _animator = gameObject.GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        _respawnTimer.OnTimerCompleted -= Respawn;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8 || collision.gameObject.CompareTag("CannonBall"))
        {
            AlmostCrumble();
        }
        else if (collision.gameObject.layer == 20)
        {
            Crumble();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.CompareTag("CannonBall"))
        {
            Crumble();
        }
    }

    public void AlmostCrumble()
    {
        _animator.SetTrigger("AlmostCrumble");
    }
    public void Crumble()
    {
        _animator.SetTrigger("Crumble");
        _collider.enabled = false;
        if (GameManager.Instance.AudioManager._soundSource.isPlaying == false)
        {
            GameManager.Instance.AudioManager.PlayASound(_crumbleSound, true, false);
        }
        if(_doRespawn) _respawnTimer.StartTimer(_respawnTime);
    }

    private void Respawn()
    {
        _animator.SetTrigger("Respawn");
        _collider.enabled = true;
    }
}

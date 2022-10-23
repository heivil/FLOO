using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    private int _playerMask = 1 << 8, _enemyMask = 1 << 9, _groundMask = 1 << 10, _bombMask = 1 << 19, _splosionDamage;
    public AudioClip _splosionSound;

    private void Awake()
    {
        if (GameManager.Instance.LessDamage)
        {
            _splosionDamage = 8;
        }else
        {
            _splosionDamage = 10;
        }
    }
    public void Dissappear()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.AudioManager.PlayASound(_splosionSound, true, true);
    }


    /// <summary>
    /// Circlecast to detect everything in explosion radius and damage/destroy them
    /// </summary>
    private void Update()
    {
        
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero, 0 , _playerMask | _enemyMask | _groundMask | _bombMask);
        for (int i = 0; i < hit.Length; i++) {
           
            if (hit[i].collider.gameObject.layer == 8 && hit[i].collider.gameObject.GetComponentInChildren<GooHitDetector>() != null)
            {
                hit[i].collider.gameObject.GetComponentInChildren<PlayerHit>().TakeTheHit(_splosionDamage, true);
            } else if (hit[i].collider.gameObject.layer == 8 && hit[i].collider.gameObject.GetComponentInChildren<FlameHitDetector>() != null)
            {
                hit[i].collider.gameObject.GetComponentInChildren<PlayerHit>().TakeTheHit(_splosionDamage, true);
            }
            else if (hit[i].collider.gameObject.layer == 9 && hit[i].collider.gameObject.GetComponent<BadGuyUnitBase>() != null)
            {
                hit[i].collider.gameObject.GetComponent<BadGuyUnitBase>().Die();

            }
            else if (hit[i].collider.gameObject.GetComponent<CrumblingRock>() != null)
            {
                hit[i].collider.gameObject.GetComponent<CrumblingRock>()._animator.SetTrigger("AlmostCrumble");
                hit[i].collider.gameObject.GetComponent<CrumblingRock>().Crumble();
            }else if (hit[i].collider.gameObject.GetComponent<Bomb>() != null)
            {
                hit[i].collider.gameObject.GetComponent<Bomb>().Explode();
            }else if (hit[i].collider.gameObject.GetComponent<AcidContainer>() != null)
            {
                hit[i].collider.gameObject.GetComponent<AcidContainer>().CrackOpen();
            }
        }
    }
}

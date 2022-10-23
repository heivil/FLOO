using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonButton : RedButton
{
    protected Animator _animator;
    public Cannon _cannon;
    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            _cannon.FireCannon();
            _animator.SetBool("Pressed", true);
            GameManager.Instance.AudioManager.PlayASound(_down, false, false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
         _animator.SetBool("Pressed", false);
        GameManager.Instance.AudioManager.PlayASound(_up, false, false);
    }
}

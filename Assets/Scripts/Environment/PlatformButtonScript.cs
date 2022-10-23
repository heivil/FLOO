using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButtonScript : RedButton
{
    protected Animator _animator;
    public MovingPlatform _platform;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            if (_platform._moving == true)
            {
                _platform.ToggleOnOff(false);
            }
            else if (_platform._moving == false)
            {
                _platform.ToggleOnOff(true);
            }
            _animator.SetBool("Pressed", true);
            GameManager.Instance.AudioManager.PlayASound(_down, false, false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {

            if (_platform._moving == true)
            {
                _animator.SetBool("TurnedOn", true);
            }
            else if (_platform._moving == false)
            {
                _animator.SetBool("TurnedOn", false);
            }
            _animator.SetBool("Pressed", false);
            GameManager.Instance.AudioManager.PlayASound(_up, false, false);
        }
    }
}

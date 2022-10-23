using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButtonScript : RedButton
{
    protected Animator _animator;
    public Door _door;
    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            if (_door._open == true)
            {
                _door.ToggleDoor(false);
            }
            else if (_door._open == false)
            {
                _door.ToggleDoor(true);
            }
            _animator.SetBool("Pressed", true);
            GameManager.Instance.AudioManager.PlayASound(_down, false, false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            if (_door._open == true)
            {
                _animator.SetBool("TurnedOn", true);
            }
            else if (_door._open == false)
            {
                _animator.SetBool("TurnedOn", false);
            }
            GameManager.Instance.AudioManager.PlayASound(_up, false, false);
            _animator.SetBool("Pressed", false);

        }
    }
}

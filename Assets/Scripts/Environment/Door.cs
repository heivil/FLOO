using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool _open = false;
    private Animator _animator;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _collider = gameObject.GetComponent<BoxCollider2D>();
    }

    public void ToggleDoor(bool open)
    {
        _open = open;
        if(_open == true)
        {
            _animator.SetBool("Open", true);
            _collider.enabled = false;
        }
        else
        {
            _animator.SetBool("Open", false);
            _collider.enabled = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flood : MonoBehaviour
{
    private Goo _goo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Goo>() != null)
        {
            _goo = collision.gameObject.GetComponent<Goo>();
        }
        if (_goo != null)
        {
            _goo._jumpAmount = _goo._maxJumpAmount;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(_goo != null)
        {
            _goo._jumpAmount = _goo._maxJumpAmount;
        }
    }
}

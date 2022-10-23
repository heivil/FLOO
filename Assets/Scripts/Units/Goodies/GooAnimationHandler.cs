using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooAnimationHandler : MonoBehaviour
{
    public Goo _goo;

    public void AfterDeath()
    {
        _goo.DeathMenu();
    }

    public void PreUnhit()
    {
        _goo._animator.SetBool("Hit", false);
    }

    public void UnHang()
    {
        _goo._animator.SetBool("Hanging", false);
        _goo._rB.drag = _goo._normalDrag;
        if (GameManager.Instance.TripleJump)
        {
            _goo._jumpAmount = 2;
        }
        else
        {
            _goo._jumpAmount = 1;
        }
        _goo._justHung = true;
    }
}

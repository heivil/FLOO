using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAnimationHandler : MonoBehaviour
{
    public Flame _flame;

    public void ReReForm()
    {
        _flame.ReForm();
    }

    public void AfterDeath()
    {
        _flame.DeathMenu();
    }

    public void FakeAfterTakeOff()
    {
        _flame.AfterTakeOff();
    }

    public void PreUnHit()
    {
        _flame._animator.SetBool("Hit", false);
    }
}

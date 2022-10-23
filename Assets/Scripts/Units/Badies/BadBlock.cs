using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadBlock : BadGuyUnitBase
{

    protected override void Awake()
    {
        base.Awake();
        _rB = gameObject.GetComponent<Rigidbody2D>();
    }

}

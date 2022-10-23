using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargeter 
{
    void Target(GameObject target);
    void ClearTarget();
}

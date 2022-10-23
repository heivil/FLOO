using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    public void StopBurning()
    {
        gameObject.SetActive(false);
    }
}

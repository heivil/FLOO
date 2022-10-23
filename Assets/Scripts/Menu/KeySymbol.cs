using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySymbol : MonoBehaviour
{
    public int _keyId;

    private void OnEnable()
    {
        if(GameManager.Instance.CheckKey(_keyId) == false)
        {
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryGips : MonoBehaviour
{
    public Flyer _flyer;
    public AudioClip _gipsDeath;
    private void Awake()
    {
        if (GameManager.Instance._bumbleBee == false)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _flyer._deathSound = _gipsDeath;
        }
    }

}

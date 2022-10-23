using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidContainer : MonoBehaviour
{
    public GameObject _acid, _brokenHalf1, _brokenHalf2;
    public AudioSource _audioSource;
    public void CrackOpen()
    {
        _acid.gameObject.SetActive(true);
        _acid.transform.parent = null;
        _brokenHalf1.gameObject.SetActive(true);
        _brokenHalf1.transform.parent = null;
        _brokenHalf2.gameObject.SetActive(true);
        _brokenHalf2.transform.parent = null;
        gameObject.SetActive(false);
        _audioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 17 || collision.gameObject.GetComponent<Flame>() != null ||
            collision.gameObject.layer == 11 || collision.gameObject.layer == 14 || 
            collision.gameObject.layer == 20)
        {
            CrackOpen();
        }
    }

}

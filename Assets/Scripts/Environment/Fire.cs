using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private Timer _timer;
    private GooHitDetector _gooHit;
    public float _burnTime = 1;
    private int _fireDamage;
    public AfterBurn _afterBurn;

    private void Awake()
    {
        _timer = gameObject.AddComponent<Timer>();
        if (GameManager.Instance.LessDamage)
        {
            _fireDamage = 4;
        }else
        {
            _fireDamage = 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInChildren<GooHitDetector>() != null)
        {
            if (_gooHit == null)
            {
                _gooHit = collision.gameObject.GetComponentInChildren<GooHitDetector>();
            }
            if (_timer._isRunning == false)
            {
                _gooHit.TakeTheHit(_fireDamage, false);
                _timer.StartTimer(_burnTime);

                if(_gooHit.gameObject.GetComponentInChildren<AfterBurn>() == null)
                {
                    _afterBurn.transform.position = _gooHit.gameObject.transform.position;
                    _afterBurn.transform.parent = _gooHit.gameObject.transform;
                    _afterBurn.gameObject.SetActive(true);
                }else
                {
                    _gooHit.gameObject.GetComponentInChildren<AfterBurn>().gameObject.SetActive(true);
                }
            }
        }else if (collision.gameObject.layer == 9)
        {
            collision.gameObject.GetComponent<BadGuyUnitBase>().Die();
        }else if (collision.gameObject.layer == 10 && collision.gameObject.CompareTag("AcidContainer"))
        {
            collision.gameObject.GetComponent<AcidContainer>().CrackOpen();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Goo>() != null)
        {
            if (_gooHit == null)
            {
                _gooHit = collision.gameObject.GetComponentInChildren<GooHitDetector>();
            }
            if (_timer._isRunning == false)
            {
                _timer.StartTimer(_burnTime);
                _afterBurn.transform.position = _gooHit.gameObject.transform.position;
                _afterBurn.transform.parent = _gooHit.gameObject.transform;
                _afterBurn.gameObject.SetActive(true);
            }
        }
    }


}

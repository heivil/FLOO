using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private Goo _goo;
    public float _waterGravity = 0.5f, _waterDrag = 16, _waterMaxVelocity = 5;
    private float _ogGravity = 0, _ogDrag = 0, _ogMaxVelocity;
    public GameObject _splash;
    public AudioClip _playerSwitchSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<Goo>() != null)
        {
            _goo = collision.gameObject.GetComponent<Goo>();
            _goo._inWater = true;
            //_goo._rB.AddForce(-(_goo._rB.velocity * 100));
            _ogMaxVelocity = _goo._maxVelocity;
            _goo._maxVelocity = _waterMaxVelocity;
            _goo._rB.velocity = _goo._rB.velocity / 2;
            _splash.transform.position = _goo.transform.position;
            _splash.SetActive(true);
            
            if (_ogGravity == 0)
            {
                _ogGravity = _goo._rB.gravityScale;
                
            }
            if (_ogDrag == 0)
            {
                _ogDrag = _goo._rB.drag;
            }

            if(_goo.GetComponentInChildren<AfterBurn>() != null)
            {
                _goo.GetComponentInChildren<AfterBurn>().gameObject.SetActive(false);
            }

        }else if (collision.gameObject.GetComponent<Flame>() != null)
        {
            GameManager.Instance.LevelManager._player.SwitchUnit();
            GameManager.Instance._audioManager.PlayASound(_playerSwitchSound, true, false);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Goo>() != null)
        {
            _goo = collision.gameObject.GetComponent<Goo>();
            _goo._rB.gravityScale = _waterGravity;
            _goo._rB.drag = _waterDrag;
            _goo._jumpAmount = _goo._maxJumpAmount;
            _goo._inWater = true;
        }
        else if (collision.gameObject.GetComponent<Rigidbody2D>() != null && !collision.gameObject.tag.Equals("WaterEnemy"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            collision.gameObject.GetComponent<Rigidbody2D>().drag = 0.69f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Goo>() != null)
        {
            _goo = collision.gameObject.GetComponent<Goo>();
            _goo._rB.gravityScale = _ogGravity;
            _goo._rB.drag = _ogDrag;
            _goo._maxVelocity = _ogMaxVelocity;
            _goo._jumpAmount = _goo._maxJumpAmount - 1;
            _goo._inWater = false;

        }
        else if (collision.gameObject.GetComponent<Rigidbody2D>() != null && !collision.gameObject.tag.Equals("WaterEnemy"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            collision.gameObject.GetComponent<Rigidbody2D>().drag = 0;
        }
    }
}

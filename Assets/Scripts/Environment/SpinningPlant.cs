using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPlant : MonoBehaviour
{
    public float _maxWindForce = 40;
    private float _mathyShit, _windBoxLength;
    private BoxCollider2D _windBox;

    private void Awake()
    {
        _windBox = gameObject.GetComponent<BoxCollider2D>();
        _windBoxLength = _windBox.size.y;
    }

    /// <summary>
    /// While object with rigidbody is in the trigger, add force to it that simulates wind
    /// </summary>
    /// <param name="collision">Other gameobjects collider</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)     
        {
            float dist = Vector2.Distance(transform.position, collision.transform.position);
            if (dist > _windBoxLength) dist = _windBoxLength;
            _mathyShit = dist / _windBoxLength;
            float minuser = _maxWindForce * _mathyShit;
            Vector3 toTheSide;
            if(transform.position.x > collision.transform.position.x)
            {
                toTheSide = new Vector2(-2,0);
            }else
            {
                toTheSide = new Vector2(2, 0);
            }

            if (collision.gameObject.GetComponent<Flame>() != null && collision.gameObject.GetComponent<Flame>()._flying)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * (_maxWindForce * 2 - minuser) + toTheSide);
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * (_maxWindForce - minuser) + toTheSide);
            }

            if (collision.gameObject.GetComponent<Goo>() != null)
            {
                collision.gameObject.GetComponent<Goo>()._jumpAmount = 1;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTargeter : MonoBehaviour
{
    public GameObject _unit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8 && _unit.GetComponent<SnakeBoss>() != null)
        {
            _unit.GetComponent<SnakeBoss>().Target(collision.gameObject, transform);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && _unit.GetComponent<SnakeBoss>() != null)
        {
            _unit.GetComponent<SnakeBoss>().Target(collision.gameObject, transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && _unit.GetComponent<SnakeBoss>() != null)
        {
            _unit.GetComponent<SnakeBoss>().ClearTarget();
        }
    }

}

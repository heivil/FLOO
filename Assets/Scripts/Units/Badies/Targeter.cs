using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    private float _maxMoveWidth, _maxMoveHeight;
    public GameObject _unit;
    private void Awake()
    {
        _maxMoveHeight = gameObject.GetComponent<BoxCollider2D>().size.y / 2;
        _maxMoveWidth = gameObject.GetComponent<BoxCollider2D>().size.x / 2;
    }

    /// <summary>
    /// Targets player when player enters patrol trigger area
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8 && _unit.GetComponent<ITargeter>() != null)
        {
            if (collision.gameObject.GetComponent<Goo>() != null)
            {
                _unit.GetComponent<ITargeter>().Target(collision.gameObject);
            }
        }
    }

    /// <summary>
    /// Stops targeting player when player exits patrol trigger area
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (_unit.GetComponent<ITargeter>() != null)
            {
                _unit.GetComponent<ITargeter>().ClearTarget();
            }
        }
    }

    /// <summary>
    /// Determines random point to move towards, within the patrol area trigger
    /// </summary>
    /// <returns>Position to move towards</returns>
    public Vector2 GetRandomPoint()
    {
        Vector2 _moveHere = transform.position - new Vector3(Random.Range(-_maxMoveWidth, _maxMoveWidth), Random.Range(-_maxMoveHeight, _maxMoveHeight), 0);
        return _moveHere;
    }

}

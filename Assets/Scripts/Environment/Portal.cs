using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal _otherPortal;
    public List<GameObject> _list;
    private Goo _goo;
    private Flame _flame;

    private void Start()
    {
        if(_goo == null) _goo = GameManager.Instance.LevelManager._player._goo;
        if(_flame == null) _flame = GameManager.Instance.LevelManager._player._flame;

    }


    /// <summary>
    /// Teleport other game object to the other portal paired to this one. Add that object to list so it does not teleport when entering position of the other portal
    /// </summary>
    /// <param name="collision">other collider</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_flame != null && collision.gameObject == _flame.gameObject)
        {
            if (_goo != null && !_list.Contains(_goo.gameObject) && !_list.Contains(collision.gameObject))
            {
                _otherPortal._list.Add(collision.gameObject);
                collision.gameObject.transform.position = _otherPortal.gameObject.transform.position;
            }
        }else if (_goo != null && collision.gameObject == _goo.gameObject)
        {
            if (_flame != null && !_list.Contains(_flame.gameObject) && !_list.Contains(collision.gameObject))
            {
                _otherPortal._list.Add(collision.gameObject);
                collision.gameObject.transform.position = _otherPortal.gameObject.transform.position;
                if (collision.gameObject.GetComponent<Goo>() != null && _goo != null)
                {
                    if (_goo._jumpAmount == 0) _goo._jumpAmount = 1;
                }
            }
        }
        else if(!_list.Contains(collision.gameObject))
        {
            _otherPortal._list.Add(collision.gameObject);
            collision.gameObject.transform.position = _otherPortal.gameObject.transform.position;
        }
    }

    /// <summary>
    /// WHen object exits portal, remove that object from list so it can be teleported again
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_goo != null && collision.gameObject == _goo.gameObject)
        {
            if (collision.gameObject.activeInHierarchy) 
            {
                _list.Remove(collision.gameObject);
                if (_list.Contains(_flame.gameObject)) _list.Remove(_flame.gameObject);
            }
        }else if (_flame != null && collision.gameObject == _flame.gameObject)
        {
            if (collision.gameObject.activeInHierarchy)
            {
                _list.Remove(collision.gameObject);
                if (_list.Contains(_goo.gameObject)) _list.Remove(_goo.gameObject);
            }
        }
        else
        {
            _list.Remove(collision.gameObject);
        }
        
    }
}

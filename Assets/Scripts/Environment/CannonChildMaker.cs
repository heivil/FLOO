using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonChildMaker : MonoBehaviour
{

    //This script makes the player cannons child when touching, so if cannon moves the player moves with it

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.activeInHierarchy && collision.gameObject.layer == 8)
        {
            if (collision.gameObject.transform.parent != null)
            {
                collision.gameObject.transform.parent.transform.parent = transform;
            }
            else
            {
                collision.gameObject.transform.parent = transform;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.activeInHierarchy && collision.gameObject.layer == 8)
        {
            if (collision.gameObject.transform.parent.transform.parent == transform)
            {
                collision.gameObject.transform.parent.transform.parent = null;
            }
            else if (collision.gameObject.transform.parent == transform)
            {
                collision.gameObject.transform.parent = null;
            }
        }
    }
}

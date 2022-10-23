using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperWallDetector : MonoBehaviour
{
    private Jumper _unit;
    private void Awake()
    {
        _unit = gameObject.GetComponentInParent<Jumper>();
    }

    /// <summary>
    /// If a wall is detected in fron of jumper enemy, changes that units direction away from wall
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9)
        {
            _unit.ChangeDir();
        }
    }
}

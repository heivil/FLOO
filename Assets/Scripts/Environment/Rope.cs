using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Rigidbody2D _stoneRB;
    public Burn _burn;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Flame>() != null)
        {
            Snap(collision.gameObject.transform.position.y);
        }
    }
    public void Snap(float height)
    {
        _stoneRB.gravityScale = 1.1f;
        _stoneRB.constraints = ~RigidbodyConstraints2D.FreezePosition;
        _burn.gameObject.transform.position = new Vector3(transform.position.x, height, 0);
        _burn.gameObject.transform.parent = null;
        _burn.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Snap()
    {
        _stoneRB.gravityScale = 1.1f;
        _stoneRB.constraints = ~RigidbodyConstraints2D.FreezePosition;
        _burn.gameObject.transform.parent = null;
        _burn.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}

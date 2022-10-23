using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerFallDecider : MonoBehaviour
{
    private Crawler _crawler;
    private Vector3 _boxCastSize = new Vector3(0.8f, 0.5f, 0);
    private int _groundLayerMask = 1 << 10;

    private void Awake()
    {
        _crawler = GetComponentInParent<Crawler>();
    }
    private void Update()
    {
        RaycastHit2D boxCast = Physics2D.BoxCast(transform.position, _boxCastSize, 0, Vector2.zero, 0, _groundLayerMask);
        if(boxCast.collider == null)
        {
            _crawler._rB.gravityScale = 1;
        }
        else
        {
            if (boxCast.collider.gameObject.CompareTag("OneSided") == false)
            {
                _crawler._rB.gravityScale = 0;
            }
            else if (boxCast.collider.gameObject.CompareTag("OneSided") == true)
            {
                //omg XDDDD
                if (boxCast.collider.transform.rotation.z == 0 && _crawler.transform.position.y > boxCast.collider.transform.position.y ||
                    boxCast.collider.transform.rotation.z == -1 && _crawler.transform.position.y < boxCast.collider.transform.position.y ||
                    boxCast.collider.transform.rotation.z == 0.7f && _crawler.transform.position.x < boxCast.collider.transform.position.x ||
                    boxCast.collider.transform.rotation.z == -0.7f && _crawler.transform.position.x > boxCast.collider.transform.position.x)
                {
                    _crawler._rB.gravityScale = 0;
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {   
        if (collision.gameObject.layer == 8 && collision.gameObject.GetComponent<Flame>() != null || 
            collision.gameObject.layer == 11 || collision.gameObject.layer == 17 || collision.gameObject.layer == 9 || collision.gameObject.layer == 20)
        {
            if (_crawler._turnTimer._isRunning == false)
            {
                _crawler.ChangeDir();
            }
            
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GameManager.Instance.LevelManager.StartEndFeedback();
            GameManager.Instance.LevelManager._player.gameObject.SetActive(false);
        }
    }
}

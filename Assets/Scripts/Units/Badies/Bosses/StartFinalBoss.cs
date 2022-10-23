using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinalBoss : MonoBehaviour
{
    private FinalBoss _boss;
    private BoxCollider2D _startCollider;
    private void Awake()
    {
        _startCollider = gameObject.GetComponent<BoxCollider2D>();
        _boss = GetComponentInChildren<FinalBoss>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _boss.StartBossFight();
        _startCollider.enabled = false;
    }
}

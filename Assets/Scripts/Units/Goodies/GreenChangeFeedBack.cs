using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenChangeFeedBack : MonoBehaviour
{
    private Timer _timer;
    public float _lifeTime = 0.5f;
    private Player _player;

    private void Awake()
    {
        _timer = gameObject.AddComponent<Timer>();
        _timer.OnTimerCompleted += GoAway;
        _player = GameManager.Instance.LevelManager._player;
    }

    /// <summary>
    /// Shows small number to indicate how much currency is gained or lost. Dissapears after set time
    /// </summary>
    private void OnEnable()
    {
        _timer.StartTimer(_lifeTime);
        transform.position = _player._activeUnit.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
    }
    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= GoAway;
    }

    private void GoAway()
    {
        gameObject.SetActive(false);
    }
}

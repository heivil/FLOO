using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlGuides : MonoBehaviour
{
    public GameObject _flameAimArrow;
    public GameObject _gooAimArrow;
    public GameObject _thingToActivate;
    private Player _player;


    private void Awake()
    {
        _player = GameManager.Instance.LevelManager._player;
    }

    void Update()
    {
        if (_player._flame.gameObject.activeSelf == true)
        {
            _thingToActivate = _flameAimArrow;
        }
        else if (_player._goo.gameObject.activeSelf == true)
        {
            _thingToActivate = _gooAimArrow;
        }
    }
}

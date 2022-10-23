using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FingerPos : MonoBehaviour
{
    public GameObject _startPos, _currentPos;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }


    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPos.transform.position = touch.position; 
                    _currentPos.transform.position = touch.position;
                    _startPos.SetActive(true);
                    break;

                case TouchPhase.Moved:
                    _currentPos.transform.position = touch.position;
                    break;

                case TouchPhase.Ended:
                    _startPos.SetActive(false);
                    break;
            }
        }
    }
}

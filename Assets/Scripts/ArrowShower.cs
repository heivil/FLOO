using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShower : MonoBehaviour
{
    public LevelManager _levelManager;
    public GameObject _buttonShowingArrows, _greenShowingArrows;
    private int _comparedGreenAmount;
    private bool _shownPlus, _shownMinus, _shownButton;

    private void Awake()
    {
        _comparedGreenAmount = _levelManager.CollectedGreens;
    }

    private void Update()
    {
        if (_comparedGreenAmount < _levelManager.CollectedGreens)
        {
            _comparedGreenAmount = _levelManager.CollectedGreens;
            _greenShowingArrows.SetActive(true);
            
        }
        else if (_comparedGreenAmount > _levelManager.CollectedGreens)
        {
            
            _comparedGreenAmount = _levelManager.CollectedGreens;
            _greenShowingArrows.SetActive(true);
            
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && _shownButton == false)
        {
            _buttonShowingArrows.gameObject.SetActive(true);
            _shownButton = true;
        }
    }

    public void ButtonArrowsAway()
    {
        _buttonShowingArrows.gameObject.SetActive(false);
    }

}

using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ExtraLevelButton : MonoBehaviour
{
    public int _levelNumber;
    public GameObject _numbers, _lock, _ten;

    private void OnEnable()
    {
        if (_levelNumber - 1 > GameManager.Instance._beatenExtraLevels)
        {
            gameObject.GetComponent<Button>().interactable = false;
            _ten.SetActive(false);
            _lock.gameObject.SetActive(false);
            _numbers.gameObject.SetActive(false);
        }
        else if (_levelNumber > GameManager.Instance._unlockedExtraLevels)
        {
            _lock.gameObject.SetActive(true);
            _numbers.gameObject.SetActive(false);
            gameObject.GetComponent<Button>().interactable = true;
            _ten.SetActive(true);
        }
        else
        {
            _lock.gameObject.SetActive(false);
            _numbers.gameObject.SetActive(true);
            gameObject.GetComponent<Button>().interactable = true;
            if (_levelNumber > GameManager.Instance._beatenExtraLevels )
            {
                _ten.SetActive(true);
            }else
            {
                _ten.SetActive(false);
            }
            
        }
    }
}

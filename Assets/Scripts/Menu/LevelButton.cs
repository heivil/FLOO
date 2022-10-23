using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int _levelNumber;
    public GameObject _numbers;
    public GameObject _ten;

    private void OnEnable()
    {
        if (_levelNumber <= GameManager.Instance.CompletedLevels + 1)
        {
            _numbers.gameObject.SetActive(true);
            if (_levelNumber == GameManager.Instance.CompletedLevels + 1)
            {
                _ten.SetActive(true);
            }
            else
            {
                _ten.SetActive(false);
            }
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            _numbers.gameObject.SetActive(false);
            _ten.SetActive(false);
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}

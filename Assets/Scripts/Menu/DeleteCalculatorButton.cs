using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteCalculatorButton : MonoBehaviour
{
    public int _sum;
    public DeleteMathTest _test;

    public void DeleteData()
    {
        if (_sum == _test.Answer)
        {
            GameManager.Instance._startAtMapExtra = false;
            GameManager.Instance.ClearData();
            GameManager.Instance.LoadData();
            SceneManager.LoadScene(0);
        }else
        {
            if (_test.gameObject.activeSelf)
            {
                _test.gameObject.SetActive(false);
            }
        }
    }
}

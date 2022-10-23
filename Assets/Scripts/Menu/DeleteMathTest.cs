using TMPro;
using UnityEngine;

public class DeleteMathTest : MonoBehaviour
{
    private string _calculation;
    private int _answer;
    public TMP_Text _calculationText;

    private void OnEnable()
    {
        GenerateMath();
        _calculationText.text = _calculation;
    }

    public int Answer
    {
        get { return _answer; }
        private set { _answer = value; }
    }

    private void GenerateMath()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                _calculation = "1 + 1 =";
                Answer = 2;
                break;
            case 1:
                _calculation = "2 + 2 =";
                Answer = 4;
                break;
            case 2:
                _calculation = "3 + 3 =";
                Answer = 6;
                break;
            default:
                _calculation = "1 + 1 =";
                Answer = 2;
                break;
        }
    }
}

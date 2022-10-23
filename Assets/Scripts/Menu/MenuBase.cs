using UnityEngine;
using TMPro;

public class MenuBase : MonoBehaviour
{
    public TMP_Text _savedGreens;
    protected RepeatingTimer _flashTimer;
    public Color _green, _red;

    private void OnDisable()
    {
        if (_savedGreens != null)
        {
            _savedGreens.color = _green;
            _flashTimer.StopTimer();
        }
    }
    public void UpdateSavedGreens()
    {
        if (GameManager.Instance.SavedGreens < 0) GameManager.Instance.SavedGreens = 0;
        _savedGreens.text = GameManager.Instance.SavedGreens.ToString();
        
    }

    protected void StartFlash()
    {
        _flashTimer.StartTimer(0.25f, 9);
        _savedGreens.color = _red;
    }

    protected void FlashGreenRed()
    {
        if (_savedGreens.color == _green)
        {
            _savedGreens.color = _red;
        }
        else if (_savedGreens.color == _red)
        {
            _savedGreens.color = _green;
        }
    }
}

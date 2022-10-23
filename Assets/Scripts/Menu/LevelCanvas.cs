using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelCanvas : MonoBehaviour
{
    public GameObject _pauseMenu;
    public TMP_Text _greenCounter;
    public GameObject _deathMenu;
    public Button _pauseButton, _switchButton;
    public Sprite _blueDeathScreen, _orangeDeathScreen;
    public Image _deathScreenImage;
    public TMP_Text _greens;
    protected RepeatingTimer _flashTimer;
    public Color _green, _red;
    public GameObject _fingerBalls;

    private void Awake()
    {
        _flashTimer = gameObject.AddComponent<RepeatingTimer>();
        _flashTimer.OnTimerCompleted += FlashGreenRed;
    }

    private void OnDestroy()
    {
        _flashTimer.OnTimerCompleted -= FlashGreenRed;
    }

    public void OpenPauseMenu()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
        GameManager.Instance._paused = true;
        DisableGameButtons();
    }

    public void ClosePauseMenu()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        GameManager.Instance._paused = false;
        DisableGameButtons();
    }

    /// <summary>
    /// level reload
    /// </summary>
    public void Reload()
    {
        _pauseMenu.SetActive(false);
        GameManager.Instance._restartCount++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        GameManager.Instance._paused = false;
        GameManager.Instance.SavedGreens -= 10;
        GameManager.Instance.SaveData();
        if(GameManager.Instance._restartCount >= GameManager.Instance._showAdNumber)
        {
            GameManager.Instance._restartCount = 0;
            GameManager.Instance._showAdNumber++;
            GameManager.Instance.AdCounter(0, false);
        }   
    }

    public void BackToMenu()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        GameManager.Instance._paused = false;
        if (GameManager.Instance._showAdNumber == 5)
        {
            GameManager.Instance.AdCounter(1, true);
        }
        GameManager.Instance.SaveData();
        SceneManager.LoadScene(0);
    }

    public void SwitchButton()
    {
        GameManager.Instance.LevelManager._player.SwitchUnit();
    }

    public void UpdateGreens(int amount)
    {
        _greenCounter.text = amount.ToString();
    }

    public void OpenDeathMenu()
    {
        if (GameManager.Instance.LevelManager._player._activeUnit.GetComponent<Goo>() != null)
        {
            _deathScreenImage.sprite = _blueDeathScreen;
        }else if (GameManager.Instance.LevelManager._player._activeUnit.GetComponent<Flame>() != null)
        {
            _deathScreenImage.sprite = _orangeDeathScreen;
        }
        _deathMenu.SetActive(true);
        Time.timeScale = 0;
        GameManager.Instance._paused = true;
        DisableGameButtons();
    }

    private void DisableGameButtons() 
    {
        if(GameManager.Instance._paused == true)
        {
            _pauseButton.gameObject.SetActive(false);
            _switchButton.gameObject.SetActive(false);
        }
        else
        {
            _pauseButton.gameObject.SetActive(true);
            _switchButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Currency flashes red when there is not enough collected to fly
    /// </summary>
    public void StartFlash()
    {
        _flashTimer.StartTimer(0.25f, 9);
        _greens.color = _red;
    }

    /// <summary>
    /// Changes flash color every time flash timer completes
    /// </summary>
    private void FlashGreenRed()
    {
        if (_greens.color == _green)
        {
            _greens.color = _red;
        }
        else if (_greens.color == _red)
        {
            _greens.color = _green;
        }
    }

}

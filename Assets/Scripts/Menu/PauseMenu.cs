using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuBase
{
    public Image _musicSymbol, _sfxSymbol, _mainMenuImage;
    public Sprite[] _symbolSprites;
    public TMP_Text _levelNumber;
    public MainMenu _mainMenu;
    public GameObject _confirmScreen, _fingerPosObject, _confirmMathScreen;
    public Sprite _towerNight;
    public Image _fingerPosImg;
    public Sprite _fingerPosOn, _fingerPosOff;

    private void Start()
    {
        if(_mainMenu != null) {
            _flashTimer = gameObject.AddComponent<RepeatingTimer>();
            _flashTimer.OnTimerCompleted += FlashGreenRed;
        }
    }

    private void OnEnable()
    {
        if (_levelNumber != null)
            _levelNumber.text = "LVL " + SceneManager.GetActiveScene().buildIndex.ToString();

        if (GameManager.Instance.AudioManager._sfxMuted == true)
        {
            _sfxSymbol.sprite = _symbolSprites[1];
        }
        else
        {
            _sfxSymbol.sprite = _symbolSprites[0];
        }

        if (GameManager.Instance.AudioManager._musicMuted == true)
        {
            _musicSymbol.sprite = _symbolSprites[3];
        }
        else
        {
            _musicSymbol.sprite = _symbolSprites[2];

        }
        if (_fingerPosImg != null && _fingerPosOn != null && _fingerPosOff != null)
        {
            if (GameManager.Instance._fingerPosOn == true)
            {
                _fingerPosImg.sprite = _fingerPosOn;
            }
            else
            {
                _fingerPosImg.sprite = _fingerPosOff;
            }
        }
    }

    private void OnDestroy()
    {
        if (_flashTimer != null) {
            _flashTimer.OnTimerCompleted -= FlashGreenRed;
        }
    }

    public void MuteSFX()
    {
        if (GameManager.Instance.AudioManager._sfxMuted == false)
        {
            GameManager.Instance.AudioManager._audioMixer.SetFloat("SFXVol", -80.0f);
            GameManager.Instance.AudioManager._sfxMuted = true;
            _sfxSymbol.sprite = _symbolSprites[1];
        }
        else
        {
            GameManager.Instance.AudioManager._audioMixer.SetFloat("SFXVol", 0);
            GameManager.Instance.AudioManager._sfxMuted = false;
            _sfxSymbol.sprite = _symbolSprites[0];
        }
    }

    public void MuteMusic()
    {
        if (GameManager.Instance.AudioManager._musicMuted == false)
        {
            GameManager.Instance.AudioManager._audioMixer.SetFloat("MusicVol", -80.0f);
            GameManager.Instance.AudioManager._musicMuted = true;
            _musicSymbol.sprite = _symbolSprites[3];
        }
        else
        {
            GameManager.Instance.AudioManager._audioMixer.SetFloat("MusicVol", 0);
            GameManager.Instance.AudioManager._musicMuted = false;
            _musicSymbol.sprite = _symbolSprites[2];
        }
    }

    public void ToggleFingerPos()
    {
        if(GameManager.Instance._fingerPosOn == true)
        {
            if (_fingerPosObject != null)
            {
                _fingerPosObject.SetActive(false);
                GameManager.Instance._fingerPosOn = false;
                _fingerPosImg.sprite = _fingerPosOff;
            }
        }else
        {
            if (_fingerPosObject != null)
            {
                _fingerPosObject.SetActive(true);
                GameManager.Instance._fingerPosOn = true;
                _fingerPosImg.sprite = _fingerPosOn;
            }
        }
    }

    public void ConfirmScreen()
    {
        if (_confirmScreen.activeSelf)
        {
            _confirmScreen.SetActive(false);
        }
        else
        {
            _confirmScreen.SetActive(true);
        }
    }

    public void ConfirmMathScreen()
    {
        if (_confirmMathScreen.activeSelf)
        {
            _confirmMathScreen.SetActive(false);
        }
        else
        {
            _confirmMathScreen.SetActive(true);
        }
    }

}

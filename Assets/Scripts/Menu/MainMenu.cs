using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class MainMenu : MenuBase
{
    
    public GameObject _mainScreen;
    public GameObject _settingsScreen;
    public GameObject _mapScreen;
    public StoreMenu _storeScreen;
    public GameObject _backButton;
    public GameObject _storeButton;
    public StoryIntro _storyIntro;
    public UpgradesScreen _upgradesScreen;
    public GameObject _upgradesButton;
    public AudioClip _menuMusic, _introMusic;
    public GameObject _adButton;
    public GameObject _creditsScreen;
    public GameObject _optionsScreen;
    public Image _mainImage;
    public Sprite _morning;
    public GameObject _adHighLight, _powerUpHighLight;
    public GameObject _collectedGreensObj;
    private int _flasInt;
    private bool _doFlash = true, _adHiglightChecker = false;
    private string _iOsRewardID = "Rewarded_iOS", _androidRewardID = "Rewarded_Android"; 

    private void Awake()
    {
        UpdateSavedGreens();
    }

    private void OnEnable()
    {
        GameManager.Instance.AudioManager.ChangeMusic(_menuMusic);
        if (GameManager.Instance.CompletedLevels == 80)
        {
            _mainImage.sprite = _morning; 
        }
        GameManager.Instance._mainmenu = this;

        DetermineUpgradeFlash();
        if (GameManager.Instance.SavedGreens < 10)
        {
            AdHiglightOn();
        }else if (GameManager.Instance.SavedGreens >= _flasInt && _doFlash)
        {
            _powerUpHighLight.SetActive(true);
        }

        if (GameManager.Instance._startAtMapExtra == true)
        {
            _mapScreen.SetActive(true);
            _mainScreen.SetActive(false);
            _backButton.SetActive(true);
            _storeButton.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Highlights reward ad button 
    /// </summary>
    public void AdHiglightOn()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Advertisement.IsReady(_androidRewardID))
            {
                _adHighLight.SetActive(true);
                _adHiglightChecker = false;
            }
            else
            {
                _adHiglightChecker = true;
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Advertisement.IsReady(_iOsRewardID))
            {
                _adHighLight.SetActive(true);
                _adHiglightChecker = false;
            }
            else
            {
                _adHiglightChecker = true;
            }
        }
    }

    private void Update()
    {
        if(_adHiglightChecker)
        {
            AdHiglightOn();
        }
    }

    /// <summary>
    /// Highlight button that opens upgrade screen when player has enough currency to buy one
    /// </summary>
    private void DetermineUpgradeFlash()
    {
        if(!GameManager.Instance._moreStartGreenPaid || !GameManager.Instance._noFlyConsumePaid || !GameManager.Instance._lessDamagePaid)
        {
            _flasInt = 210;
        }else if (!GameManager.Instance._lessImpactDamagePaid)
        {
            _flasInt = 310;
        }else if (!GameManager.Instance._tripleJumpPaid)
        {
            _flasInt = 410;
        }else if (!GameManager.Instance._moreGreenPaid)
        {
            _flasInt = 510;
        }else
        {
            _doFlash = false;
        }
    }

    private void OnDisable()
    {
        GameManager.Instance._mainmenu = null;
    }

    public void StartGame()
    {
        if (GameManager.Instance.PlayIntro == true)
        {
            _mainScreen.SetActive(false);
            _backButton.SetActive(false);
            _adButton.gameObject.SetActive(false);
            _upgradesButton.gameObject.SetActive(false);
            _collectedGreensObj.SetActive(false);
            _storyIntro.gameObject.SetActive(true);
            GameManager.Instance.PlayIntro = false;
            GameManager.Instance.AudioManager.ChangeMusic(_introMusic);

        }
        else
        {
            _mapScreen.SetActive(true);
            _mainScreen.SetActive(false);
            _backButton.SetActive(true);
            _storeButton.gameObject.SetActive(false);
        }
    }

    public void OpenOptions()
    {
        _optionsScreen.SetActive(true);
        _mainScreen.SetActive(false);
    }

    public void CloseOptions()
    {
        _optionsScreen.SetActive(false);
        _mainScreen.SetActive(true);
        _backButton.SetActive(false);
    }

    public void OpenSettings()
    {
        _settingsScreen.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsScreen.SetActive(false);
    }

    public void MenuBackButton()
    {

        if (_upgradesScreen.gameObject.activeSelf == true)
        {
            CloseUpgradesScreen();
        }
        else if (_storeScreen.gameObject.activeSelf == true)
        {
            CloseStoreScreen();
        }
        else if(_mapScreen.activeSelf == true)
        {
            CloseMap();
        } 
        else if(_creditsScreen.activeSelf == true)
        {
            CloseCreditsScreen();
        } else if (_settingsScreen.activeSelf == true)
        {
            CloseSettings();
        }else if(_optionsScreen.activeSelf == true)
        {
            CloseOptions();
        }

    }

    public void CloseMap()
    {
        _mapScreen.SetActive(false);
        _mainScreen.SetActive(true);
        _storeScreen.gameObject.SetActive(false);
        _backButton.SetActive(false);
        _storeButton.gameObject.SetActive(true);
    }


    public void OpenStoreScreen()
    {
        _storeScreen.gameObject.SetActive(true);
        _storeButton.SetActive(false);
    }

    public void CloseStoreScreen()
    {
        _storeScreen.gameObject.SetActive(false);
        _storeButton.SetActive(true);
        if (_mapScreen.activeSelf == false)
        {
            _backButton.SetActive(false);
        }
    }

    public void OpenUpgradesScreen()
    {
        if (_powerUpHighLight.activeInHierarchy)
        {
            _powerUpHighLight.SetActive(false);
        }

        _upgradesScreen.gameObject.SetActive(true);
        _backButton.SetActive(true);
        _upgradesButton.SetActive(false);
        _storeButton.SetActive(false);
    }

    public void CloseUpgradesScreen()
    {
        GameManager.Instance.SaveData();
        _upgradesScreen.gameObject.SetActive(false);
        _upgradesButton.SetActive(true);
        if (_mapScreen.activeSelf == false)
        {
            _backButton.SetActive(false);
        }
        _storeButton.SetActive(true);
        if (_mapScreen.activeInHierarchy == false)
        {
            _backButton.SetActive(false);
        }
    }

    public void OpenCreditsScreen()
    {
        _creditsScreen.gameObject.SetActive(true);
    }

    public void CloseCreditsScreen()
    {
        _creditsScreen.gameObject.SetActive(false);
    }

    public void IWantToSeeAds()
    {
        if (_adHighLight.activeInHierarchy)
        {
            _adHighLight.SetActive(false);
        }
        GameManager.Instance._adHandler.DisplayVideoAd();
    }


    
}

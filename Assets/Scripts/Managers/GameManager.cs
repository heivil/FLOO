using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    private LevelManager _levelManager;
    private int _savedGreens;
    public bool _paused;
    private CameraScript _camera;
    private int _completedLevels;
    public AudioManager _audioManager;
    private string _savePath;
    private bool[] _collectedKeys = new bool[] {false, false, false ,false, false, false, false, false};
    private bool _playIntro = true, _tripleJump, _lessDamage, _moreGreen, _noFlyConsume, _lessImpactDamage, _moreStartGreen;
    public bool _tripleJumpPaid, _lessDamagePaid, _moreGreenPaid, _noFlyConsumePaid, _lessImpactDamagePaid, _moreStartGreenPaid;
    public int _availableKeys, _unlockedExtraLevels, _beatenExtraLevels;
    public bool _hasRecievedEndReward = false;
    public AdHandler _adHandler;
    public MainMenu _mainmenu;
    private int _countTilAd = 0;
    public bool _hasPaidForNoAds;
    public bool _bumbleBee;
    public bool _startAtMapExtra;
    public bool _fingerPosOn;
    [HideInInspector]
    public int _restartCount, _showAdNumber;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            Instance = value;
        }
    }

    public bool TripleJump
    {
        get { return _tripleJump; }
        set { _tripleJump = value; }
    }

    public bool LessDamage
    {
        get { return _lessDamage; }
        set { _lessDamage = value; }
    }

    public bool MoreGreen
    {
        get { return _moreGreen; }
        set { _moreGreen = value; }
    }

    public bool NoFlyConsume
    {
        get { return _noFlyConsume; }
        set { _noFlyConsume = value; }
    }

    public bool LessImpactDamage
    {
        get { return _lessImpactDamage; }
        set { _lessImpactDamage = value; }
    }

    public bool MoreStartGreen
    {
        get { return _moreStartGreen; }
        set { _moreStartGreen = value; }
    }

    public bool PlayIntro
    {
        get { return _playIntro; }
        set { _playIntro = value; }
    }

    public void AddKeys(int id) 
    {
        if (_collectedKeys[id] == false)
        {
            _collectedKeys[id] = true;
        }
    }

    public bool CheckKey(int id)
    {
        return _collectedKeys[id];
    }

    public AudioManager AudioManager
    {
        get
        {
            if(_audioManager == null)
            {
                Debug.LogError("No Audiomanager!");
            }
            return _audioManager;
        }
        private set
        {
            _audioManager = value;
        }
    }

    public LevelManager LevelManager
    {
        get
        {
            if (_levelManager == null)
            {
                Debug.LogError("No LevelManager!");
            }
            return _levelManager;
        }
        set
        {
            _levelManager = value;
        }
    }
    public int SavedGreens
    {
        get { return _savedGreens; }
        set { _savedGreens = value; }
    }

    public int CompletedLevels
    {
        get { return _completedLevels; }
        set { _completedLevels = value; }
    }

    public CameraScript Camera
    {
        get { return _camera; }
        set { _camera = value; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        Application.targetFrameRate = 60;
        AudioManager = GetComponentInChildren<AudioManager>();
        DontDestroyOnLoad(this);
        _savePath = Application.persistentDataPath + "/savedGame.save";
        _showAdNumber = 5;
        LoadData();
    }

    public void SaveData()
    {
        var save = new Save()
        {
            _savedGreens = SavedGreens,
            _levelsBeaten = CompletedLevels,
            _sfxMuted = _audioManager._sfxMuted,
            _musicMuted = _audioManager._musicMuted,
            _keysCollected = _collectedKeys,
            _availableKeys = _availableKeys,
            _unlockedExtraLevels = _unlockedExtraLevels,
            _beatenExtraLevels = _beatenExtraLevels,
            _playIntro = PlayIntro,
            _tripleJump = TripleJump,
            _lessDamage = LessDamage,
            _moreGreen = MoreGreen,
            _noFlyConsume = NoFlyConsume,
            _lessImpactDamage = LessImpactDamage,
            _moreStartGreen = MoreStartGreen,
            _hasRecievedEndReward = _hasRecievedEndReward,
            _hasPaid = _hasPaidForNoAds,
            _bumbleBee = _bumbleBee,
            _fingerPos = _fingerPosOn,
            _tripleJumpPaidS = _tripleJumpPaid,
            _lessDamagePaidS = _lessDamagePaid,
            _moreGreenPaidS = _moreGreenPaid,
            _noFlyConsumePaidS = _noFlyConsumePaid,
            _lessImpactDamagePaidS = _lessImpactDamagePaid,
            _moreStartGreenPaidS = _moreStartGreenPaid
        };

        var binaryFormatter = new BinaryFormatter();

        using (var fileStream = File.Create(_savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }

        print("Progress saved.");
    }

    public void LoadData()
    {
        if (File.Exists(_savePath))
        {
            Save save;

            var binaryFormatter = new BinaryFormatter();

            using (var fileStream = File.Open(_savePath, FileMode.Open))
            {
                save = (Save)binaryFormatter.Deserialize(fileStream);
            }

            SavedGreens = save._savedGreens;
            CompletedLevels = save._levelsBeaten;
            _audioManager._musicMuted = save._musicMuted;
            _audioManager._sfxMuted = save._sfxMuted;
            if(save._keysCollected != null) _collectedKeys = save._keysCollected;
            _availableKeys = save._availableKeys;
            _unlockedExtraLevels = save._unlockedExtraLevels;
            _beatenExtraLevels = save._beatenExtraLevels;
            PlayIntro = save._playIntro;
            TripleJump = save._tripleJump;
            LessDamage = save._lessDamage;
            MoreGreen = save._moreGreen;
            NoFlyConsume = save._noFlyConsume;
            LessImpactDamage = save._lessImpactDamage;
            MoreStartGreen = save._moreStartGreen;
            _hasRecievedEndReward = save._hasRecievedEndReward;
            _hasPaidForNoAds = save._hasPaid;
            _bumbleBee = save._bumbleBee;
            _fingerPosOn = save._fingerPos;
            _tripleJumpPaid = save._tripleJumpPaidS;
            _lessDamagePaid = save._lessDamagePaidS;
            _moreGreenPaid = save._moreGreenPaidS;
            _noFlyConsumePaid = save._noFlyConsumePaidS;
            _lessImpactDamagePaid = save._lessImpactDamagePaidS;
            _moreStartGreenPaid = save._moreStartGreenPaidS;
            print("Progress loaded");
        }else
        {
            SavedGreens = 50;
            CompletedLevels = 0;
            _audioManager._musicMuted = false;
            _audioManager._sfxMuted = false;
            _collectedKeys = new bool[] { false, false, false, false, false, false, false, false };
            _availableKeys = 0;
            _unlockedExtraLevels = 0;
            _beatenExtraLevels = 0;
            PlayIntro = true;
            TripleJump = false;
            LessDamage = false;
            MoreGreen = false;
            NoFlyConsume = false;
            LessImpactDamage = false;
            MoreStartGreen = false;
            _hasRecievedEndReward = false;
            _bumbleBee = false;
            _fingerPosOn = true;
            _tripleJumpPaid = false;
            _lessDamagePaid = false;
            _moreGreenPaid = false;
            _noFlyConsumePaid = false;
            _lessImpactDamagePaid = false;
            _moreStartGreenPaid = false;
        }
    }

    public void ClearData()
    {
        if (File.Exists(_savePath))
        {
            File.Delete(_savePath);
        }
        print("Data deleted");
    }

    public void AdCounter(int i, bool canZero)
    {
        if (_hasPaidForNoAds == false)
        {
            if (i > 0)
            {
                _countTilAd += i;
                if (_countTilAd >= 4)
                {
                    _countTilAd = 0;
                    _adHandler.DisplayInterstitialAd();
                }
            }
            else if (i == 0 && canZero == false)
            {
                _adHandler.DisplayInterstitialAd();
            }
            else 
            {
                _countTilAd = 0;
            }
        }
    }
}

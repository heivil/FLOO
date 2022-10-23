using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private LevelManager _instance;
    public Player _player;
    public Transform _playerStartPos, _keySpawnPos;
    private int _collectedGreens;
    [HideInInspector]
    public CameraScript _camera;
    [HideInInspector]
    public LevelCanvas _canvas;
    public int _startingGreens = 10, _currentLevel, _keyID;
    public Timer _endTimer;
    private RandomlyRepeatingTimer _fireworkTimer;
    public float _endFeedbackTime;
    public FireWorkPool _fireWorkPool;
    private FireWork _fireWork;
    public Key _key;
    public AudioClip _levelSong, _victorySound;
    public bool _keyCollected = false;
    public StoryOutro _storyOutro;
    public bool _levelEnded;
    private int _endTouchCount = 0;

    public int CollectedGreens
    {
        get
        { return _collectedGreens;}
        private set
        { _collectedGreens = value;}
    }

    private void Awake()
    {
        _endTimer = gameObject.AddComponent<Timer>();
        _endTimer.OnTimerCompleted += EndLevel;
        _fireworkTimer = gameObject.AddComponent<RandomlyRepeatingTimer>();
        _fireworkTimer.OnTimerCompleted += FireWork;
        Time.timeScale = 1;
        InitLevel();
    }

    protected virtual void OnDestroy()
    {
        _endTimer.OnTimerCompleted -= EndLevel;
        _fireworkTimer.OnTimerCompleted -= FireWork;
    }
    private void InitLevel()
    {
        GameManager.Instance.LevelManager = this;
        if (_player == null)
        {
            Debug.LogError("Add a player to LevelManager please!");
        }
        else
        {
            _player.gameObject.transform.position = _playerStartPos.position;
        }
        
        if (_camera == null)
        {
            _camera = FindObjectOfType<CameraScript>();
        }
        if(_canvas == null)
        {
            _canvas = FindObjectOfType<LevelCanvas>();
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _startingGreens = 1;
        }
        else if (GameManager.Instance.MoreStartGreen)
        {
            _startingGreens += 4;
        }
        _collectedGreens = _startingGreens;
        _canvas.UpdateGreens(CollectedGreens);
        GameManager.Instance.AudioManager.ChangeMusic(_levelSong);
        GameManager.Instance._paused = false;

        if (_key != null)
        {
            Instantiate(_key);
        }

        if (_canvas._fingerBalls != null)
        {
            if (GameManager.Instance._fingerPosOn)
            {
                _canvas._fingerBalls.SetActive(true);
            }
            else
            {
                _canvas._fingerBalls.SetActive(false);
            }
        }
    }

    public void StartEndFeedback()
    {
        
        _levelEnded = true;
        if (_player.Alive)
        {
            GameManager.Instance._audioManager.StopMusic();
            GameManager.Instance.AudioManager.PlayASound(_victorySound, false, false);
            if(GameManager.Instance.SavedGreens < 0)
            {
                GameManager.Instance.SavedGreens = 0;
            }
            GameManager.Instance.SavedGreens += CollectedGreens;
            if (_key != null && _keyCollected == true)
            {
                
                GameManager.Instance.AddKeys(_keyID);
                GameManager.Instance._availableKeys++;
            }

            _currentLevel = SceneManager.GetActiveScene().buildIndex;

            if (GameManager.Instance.CompletedLevels < _currentLevel && _currentLevel <= 80)
            {
                GameManager.Instance.CompletedLevels = _currentLevel;
            }else if (_currentLevel > 80)
            {
                switch(_currentLevel)
                {
                    case 81:
                        if(GameManager.Instance._beatenExtraLevels < 1)
                            GameManager.Instance._beatenExtraLevels = 1;
                        break;
                    case 82:
                        if (GameManager.Instance._beatenExtraLevels < 2)
                            GameManager.Instance._beatenExtraLevels = 2;
                        break;
                    case 83:
                        if (GameManager.Instance._beatenExtraLevels < 3)
                            GameManager.Instance._beatenExtraLevels = 3;
                        break;
                    case 84:
                        if (GameManager.Instance._beatenExtraLevels < 4)
                            GameManager.Instance._beatenExtraLevels = 4;
                        break;
                    case 85:
                        if (GameManager.Instance._beatenExtraLevels < 5)
                            GameManager.Instance._beatenExtraLevels = 5;
                        break;
                    case 86:
                        if (GameManager.Instance._beatenExtraLevels < 6)
                            GameManager.Instance._beatenExtraLevels = 6;
                        break;
                    case 87:
                        if (GameManager.Instance._beatenExtraLevels < 7)
                            GameManager.Instance._beatenExtraLevels = 7;
                        break;
                    case 88:
                        if (GameManager.Instance._beatenExtraLevels < 8)
                            GameManager.Instance._beatenExtraLevels = 8;
                        break;
                    default:
                        GameManager.Instance._beatenExtraLevels = 0;
                        break;
                }
            }

            GameManager.Instance.SaveData();
            _endFeedbackTime = _collectedGreens * 0.1f + 2;
            _fireworkTimer.StartTimer(0.02f, 0.1f, _collectedGreens);
            _endTimer.StartTimer(_endFeedbackTime);
        }
        else
        {
            _canvas.OpenDeathMenu();
        }

    }

    private void Update()
    {
        if (_endTimer._isRunning)
        {
            if(Input.touchCount < 0)
            {
                Touch touch = Input.GetTouch(0);
                if (TouchPhase.Began == 0)
                {
                    _endTouchCount++;
                }
            }else if (Input.GetMouseButtonDown(0))
            {
                _endTouchCount++;
            }

            if (_endTouchCount > 1)
            {
                EndLevel();
            }
        }
    }

    private void FireWork()
    {
        _fireWork = _fireWorkPool.GetPooledObject();
        _fireWork.transform.position = _camera.transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-6, 7));
        _fireWork.transform.parent = transform;
    }

    public void EndLevel()
    {

        if (SceneManager.GetActiveScene().buildIndex < 80)
        {
            GameManager.Instance._startAtMapExtra = false;
            if(GameManager.Instance._showAdNumber > 5)
            {
                if (GameManager.Instance._restartCount > 0)
                {
                    GameManager.Instance.AdCounter(1, true);
                }
            }
            else
            {
                GameManager.Instance.AdCounter(1, true);
            }
            GameManager.Instance._restartCount = 0;
            GameManager.Instance._showAdNumber = 5;
            SceneManager.LoadScene(_currentLevel + 1);
        } else if (SceneManager.GetActiveScene().buildIndex == 80)
        {
            if (_storyOutro != null)
            {
                GameManager.Instance._startAtMapExtra = false;
                _storyOutro.gameObject.SetActive(true);
                _player.gameObject.SetActive(false);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex > 80) 
        {
            GameManager.Instance._startAtMapExtra = true;
            SceneManager.LoadScene(0);
        }
        else
        {
            GameManager.Instance._startAtMapExtra = false;
            SceneManager.LoadScene(0);
        }
    }

    public void ChangeGreenAmount(int amount)
    {
        CollectedGreens += amount;
        if (CollectedGreens <= 0) 
        { 
            CollectedGreens = 0; 
        }
        
        _canvas.UpdateGreens(CollectedGreens);
    }

    public void DeathMenu()
    {
        GameManager.Instance.LevelManager.CollectedGreens = 0;
        _canvas.OpenDeathMenu();
    }
}

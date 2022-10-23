using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Map : MenuBase
{
    private int _floorCounter = 0;
    public Image[] _floorImages = new Image[9];
    public Button _upArrow, _downArrow, _leftArrow, _rightArrow;
    public Transform _upperTrans, _lowerTrans, _leftTrans, _rightTrans;
    public GameObject _firstLevelGroup, _secondLevelGroup, _thirdLevelGroup, _fourthLevelGroup, 
        _fifthLevelGroup, _sixthLevelGroup, _seventhLevelGroup, _eigthLevelGroup, _extraLevelGroup;
    public TMP_Text _keysText;
    public GameObject _keysObject;

    private void Awake()
    {
        _flashTimer = gameObject.AddComponent<RepeatingTimer>();
        _flashTimer.OnTimerCompleted += FlashGreenRed;
        CheckFloor();
    }

    private void OnDestroy()
    {
        _flashTimer.OnTimerCompleted -= FlashGreenRed;
    }

    public void FloorUp()
    {
        switch (_floorCounter)
        {
            case 1:
                _floorImages[0].gameObject.transform.position = _lowerTrans.position;
                _floorImages[1].gameObject.transform.position = transform.position;
                _floorCounter = 2;
                _downArrow.gameObject.SetActive(true);
                _secondLevelGroup.SetActive(true);
                _firstLevelGroup.SetActive(false);
                break;
            case 2:
                _floorImages[1].gameObject.transform.position = _lowerTrans.position;
                _floorImages[2].gameObject.transform.position = transform.position;
                _floorCounter = 3;
                _secondLevelGroup.SetActive(false);
                _thirdLevelGroup.SetActive(true);
                break;
            case 3:
                _floorImages[2].gameObject.transform.position = _lowerTrans.position;
                _floorImages[3].gameObject.transform.position = transform.position;
                _floorCounter = 4;
                _fourthLevelGroup.SetActive(true);
                _thirdLevelGroup.SetActive(false);
                break;
            case 4:
                _floorImages[3].gameObject.transform.position = _lowerTrans.position;
                _floorImages[4].gameObject.transform.position = transform.position;
                _floorCounter = 5;
                _leftArrow.gameObject.SetActive(true);
                _fourthLevelGroup.SetActive(false);
                _fifthLevelGroup.SetActive(true);
                break;
            case 5:
                _floorImages[4].gameObject.transform.position = _lowerTrans.position;
                _floorImages[5].gameObject.transform.position = transform.position;
                _floorCounter = 6;
                _leftArrow.gameObject.SetActive(false);
                _sixthLevelGroup.SetActive(true);
                _fifthLevelGroup.SetActive(false);
                break;
            case 6:
                _floorImages[5].gameObject.transform.position = _lowerTrans.position;
                _floorImages[6].gameObject.transform.position = transform.position;
                _floorCounter = 7;
                _sixthLevelGroup.SetActive(false);
                _seventhLevelGroup.SetActive(true);
                break;
            case 7:
                _floorImages[6].gameObject.transform.position = _lowerTrans.position;
                _floorImages[7].gameObject.transform.position = transform.position;
                _floorCounter = 8;
                _upArrow.gameObject.SetActive(false);
                _eigthLevelGroup.SetActive(true);
                _seventhLevelGroup.SetActive(false);
                break;
            case 8:
                print("no more going up sillygoose");
                break;
            default:
                _floorImages[0].gameObject.transform.position = transform.position;
                _floorCounter = 1;
                break;
        }
    }

    public void FloorDown()
    {
        switch (_floorCounter)
        {
            case 1:
                print("no more going down sillygoose");
                break;
            case 2:
                _floorImages[1].gameObject.transform.position = _upperTrans.position;
                _floorImages[0].gameObject.transform.position = transform.position;
                _floorCounter = 1;
                _downArrow.gameObject.SetActive(false);
                _upArrow.gameObject.SetActive(true);
                _secondLevelGroup.SetActive(false);
                _firstLevelGroup.SetActive(true);
                break;
            case 3:
                _floorImages[2].gameObject.transform.position = _upperTrans.position;
                _floorImages[1].gameObject.transform.position = transform.position;
                _floorCounter = 2;
                _secondLevelGroup.SetActive(true);
                _thirdLevelGroup.SetActive(false);
                break;
            case 4:
                _floorImages[3].gameObject.transform.position = _upperTrans.position;
                _floorImages[2].gameObject.transform.position = transform.position;
                _floorCounter = 3;
                _thirdLevelGroup.SetActive(true);
                _fourthLevelGroup.SetActive(false);
                break;
            case 5:
                _floorImages[4].gameObject.transform.position = _upperTrans.position;
                _floorImages[3].gameObject.transform.position = transform.position;
                _floorCounter = 4;
                _leftArrow.gameObject.SetActive(false);
                _fourthLevelGroup.SetActive(true);
                _fifthLevelGroup.SetActive(false);
                break;
            case 6:
                _floorImages[5].gameObject.transform.position = _upperTrans.position;
                _floorImages[4].gameObject.transform.position = transform.position;
                _floorCounter = 5;
                _leftArrow.gameObject.SetActive(true);
                _sixthLevelGroup.SetActive(false);
                _fifthLevelGroup.SetActive(true);
                break;
            case 7:
                _floorImages[6].gameObject.transform.position = _upperTrans.position;
                _floorImages[5].gameObject.transform.position = transform.position;
                _floorCounter = 6;
                _sixthLevelGroup.SetActive(true);
                _seventhLevelGroup.SetActive(false);
                break;
            case 8:
                _floorImages[7].gameObject.transform.position = _upperTrans.position;
                _floorImages[6].gameObject.transform.position = transform.position;
                _floorCounter = 7;
                _eigthLevelGroup.SetActive(false);
                _seventhLevelGroup.SetActive(true);
                _upArrow.gameObject.SetActive(true);
                break;
            default:
                _floorImages[0].gameObject.transform.position = transform.position;
                _floorCounter = 1;
                break;
        }
    }

    public void FloorLeft()
    {

        if (_floorCounter == 5)
        {
            _floorImages[4].gameObject.transform.position = _rightTrans.position;
            _floorImages[8].gameObject.transform.position = transform.position;
            _upArrow.gameObject.SetActive(false);
            _downArrow.gameObject.SetActive(false);
            _rightArrow.gameObject.SetActive(true);
            _leftArrow.gameObject.SetActive(false);
            _floorCounter = 9;
            _keysText.text = GameManager.Instance._availableKeys.ToString();
            _extraLevelGroup.SetActive(true);
            _fifthLevelGroup.SetActive(false);
            _keysObject.SetActive(true);
        }
            
    }

    public void FloorRight()
    {
        if (_floorCounter == 9)
        {
            _floorImages[8].gameObject.transform.position = _leftTrans.position;
            _floorImages[4].gameObject.transform.position = transform.position;
            _upArrow.gameObject.SetActive(true);
            _downArrow.gameObject.SetActive(true);
            _rightArrow.gameObject.SetActive(false);
            _leftArrow.gameObject.SetActive(true);
            _extraLevelGroup.SetActive(false);
            _fifthLevelGroup.SetActive(true);
            _floorCounter = 5;
            _keysObject.SetActive(false);
        }
    }

    private void CheckFloor()
    {
        if (GameManager.Instance._startAtMapExtra == true) 
        {
            _floorImages[5].gameObject.transform.position = _rightTrans.position;
            _floorImages[8].gameObject.transform.position = transform.position;
            _upArrow.gameObject.SetActive(false);
            _downArrow.gameObject.SetActive(false);
            _rightArrow.gameObject.SetActive(true);
            _leftArrow.gameObject.SetActive(false);
            _floorCounter = 9;
            _keysText.text = GameManager.Instance._availableKeys.ToString();
            _extraLevelGroup.SetActive(true);
            _sixthLevelGroup.SetActive(false);
            _keysObject.SetActive(true);
        }
        else if (GameManager.Instance.CompletedLevels < 10)
        {
            _floorImages[0].gameObject.transform.position = transform.position;
            _downArrow.gameObject.SetActive(false);
            _rightArrow.gameObject.SetActive(false);
            _upArrow.gameObject.SetActive(true);
            _leftArrow.gameObject.SetActive(false);
            _firstLevelGroup.SetActive(true);
            _floorCounter = 1;
        }
        else if (GameManager.Instance.CompletedLevels >= 10 && GameManager.Instance.CompletedLevels < 20)
        {
            _floorImages[1].gameObject.transform.position = transform.position;
            _downArrow.gameObject.SetActive(true);
            _rightArrow.gameObject.SetActive(false);
            _upArrow.gameObject.SetActive(true);
            _leftArrow.gameObject.SetActive(false);
            _secondLevelGroup.SetActive(true);
            _floorCounter = 2;
        }
        else if (GameManager.Instance.CompletedLevels >= 20 && GameManager.Instance.CompletedLevels < 30)
        {
            _floorImages[2].gameObject.transform.position = transform.position;
            _downArrow.gameObject.SetActive(true);
            _rightArrow.gameObject.SetActive(false);
            _upArrow.gameObject.SetActive(true);
            _leftArrow.gameObject.SetActive(false);
            _thirdLevelGroup.SetActive(true);
            _floorCounter = 3;
        }
        else if (GameManager.Instance.CompletedLevels >= 30 && GameManager.Instance.CompletedLevels < 40)
        {
            _floorImages[3].gameObject.transform.position = transform.position;
            _downArrow.gameObject.SetActive(true);
            _rightArrow.gameObject.SetActive(false);
            _upArrow.gameObject.SetActive(true);
            _leftArrow.gameObject.SetActive(false);
            _fourthLevelGroup.SetActive(true);
            _floorCounter = 4;
        }
        else if (GameManager.Instance.CompletedLevels >= 40 && GameManager.Instance.CompletedLevels < 50)
        {
            _floorImages[4].gameObject.transform.position = transform.position;
            _downArrow.gameObject.SetActive(true);
            _rightArrow.gameObject.SetActive(false);
            _upArrow.gameObject.SetActive(true);
            _leftArrow.gameObject.SetActive(true);
            _fifthLevelGroup.SetActive(true);
            _floorCounter = 5;
        }
        else if (GameManager.Instance.CompletedLevels >= 50 && GameManager.Instance.CompletedLevels < 60)
        {
            _floorImages[5].gameObject.transform.position = transform.position;
            _downArrow.gameObject.SetActive(true);
            _rightArrow.gameObject.SetActive(false);
            _upArrow.gameObject.SetActive(true);
            _leftArrow.gameObject.SetActive(false);
            _sixthLevelGroup.SetActive(true);
            _floorCounter = 6;
        }
        else if (GameManager.Instance.CompletedLevels >= 60 && GameManager.Instance.CompletedLevels < 70)
        {
            _floorImages[6].gameObject.transform.position = transform.position;
            _downArrow.gameObject.SetActive(true);
            _rightArrow.gameObject.SetActive(false);
            _upArrow.gameObject.SetActive(true);
            _leftArrow.gameObject.SetActive(false);
            _seventhLevelGroup.SetActive(true);
            _floorCounter = 7;
        }
        else if (GameManager.Instance.CompletedLevels >= 70 && GameManager.Instance.CompletedLevels <= 80)
        {
            _floorImages[7].gameObject.transform.position = transform.position;
            _downArrow.gameObject.SetActive(true);
            _rightArrow.gameObject.SetActive(false);
            _upArrow.gameObject.SetActive(false);
            _leftArrow.gameObject.SetActive(false);
            _eigthLevelGroup.SetActive(true);
            _floorCounter = 8;
        }
        
    }

    public void LoadLevel(int number)
    {
        if (number <= GameManager.Instance.CompletedLevels)
        {
            GameManager.Instance._showAdNumber = 5;
            GameManager.Instance._restartCount = 0;
            GameManager.Instance.SaveData();
            SceneManager.LoadScene(number);
        }
        else if (GameManager.Instance.SavedGreens >= 10 && GameManager.Instance.CompletedLevels < number)
        {
            GameManager.Instance.SavedGreens -= 10;
            GameManager.Instance._showAdNumber = 5;
            GameManager.Instance._restartCount = 0;
            UpdateSavedGreens();
            GameManager.Instance.SaveData();
            SceneManager.LoadScene(number);
        }
        else if(GameManager.Instance.CompletedLevels >= number - 1 && GameManager.Instance.SavedGreens < 10)
        {
            if (_flashTimer._isRunning == false)
            {
                StartFlash();
                GameManager.Instance._mainmenu.AdHiglightOn();
            }
        }
    }

    public void LoadExtraLevel(int number)
    {
        if (GameManager.Instance._availableKeys > 0 && GameManager.Instance._unlockedExtraLevels < number && number == GameManager.Instance._unlockedExtraLevels +1)
        {
            GameManager.Instance._availableKeys--;
            _keysText.text = GameManager.Instance._availableKeys.ToString();
            GameManager.Instance._unlockedExtraLevels++;
            GameManager.Instance.SavedGreens -= 10;
            UpdateSavedGreens();
            GameManager.Instance.SaveData();
            GameManager.Instance._showAdNumber = 5;
            GameManager.Instance._restartCount = 0;
            SceneManager.LoadScene(80 + number);
        } 
        else if (GameManager.Instance._unlockedExtraLevels >= number && GameManager.Instance._beatenExtraLevels >= number)
        {
            GameManager.Instance.SaveData();
            GameManager.Instance._showAdNumber = 5;
            GameManager.Instance._restartCount = 0;
            SceneManager.LoadScene(80 + number);
        }
        else if (GameManager.Instance._unlockedExtraLevels >= number && GameManager.Instance._beatenExtraLevels < number)
        {
            if (GameManager.Instance.SavedGreens < 10)
            {
                if (_flashTimer._isRunning == false)
                {
                    StartFlash();
                    GameManager.Instance._mainmenu.AdHiglightOn();
                }
            }else
            {
                GameManager.Instance.SavedGreens -= 10;
                UpdateSavedGreens();
                GameManager.Instance.SaveData();
                GameManager.Instance._showAdNumber = 5;
                GameManager.Instance._restartCount = 0;
                SceneManager.LoadScene(80 + number);
            }
        }


    }

}

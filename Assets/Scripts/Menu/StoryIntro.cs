using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StoryIntro : MonoBehaviour
{
    public Sprite[] _storyImages = new Sprite[10];
    public Image _image, _downBlack, _upBlack;
    private int _currentImageIndex;
    public GameObject _buttons;
    private Timer _lerpTimer;
    public int _lerpTime = 1;
    private Vector2 _targetSize = new Vector2(800, 1075), _upBlackStart = new Vector2(800,0), _downBlackStart = new Vector2(800, 0);
    public MainMenu _mainMenu;

    private void Awake()
    {
        _lerpTimer = gameObject.AddComponent<Timer>();
        _lerpTimer.OnTimerCompleted += HasLerped;
    }

    private void OnDestroy()
    {
        _lerpTimer.OnTimerCompleted -= HasLerped;
    }

    private void OnEnable()
    {
        _mainMenu._adHighLight.SetActive(false);
        _mainMenu._powerUpHighLight.SetActive(false);
        _image.sprite = _storyImages[0];
        _currentImageIndex = 0;
        _lerpTimer.StartTimer(_lerpTime);
    }

    private void Update()
    {
        if (_lerpTimer._isRunning)
        {
            _upBlack.rectTransform.sizeDelta = Vector2.Lerp(_upBlackStart, _targetSize, _lerpTimer.NormalizedTimeElapsed);
            _downBlack.rectTransform.sizeDelta = Vector2.Lerp(_downBlackStart, _targetSize, _lerpTimer.NormalizedTimeElapsed);
        }
    }

    private void HasLerped()
    {
        _buttons.SetActive(true);
    }

    public void NextImage()
    {
        if (_currentImageIndex == _storyImages.Length - 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            _currentImageIndex += 1;
            _image.sprite = _storyImages[_currentImageIndex];
        }
    }

    public void PreviousImage()
    {
        if (_currentImageIndex > 0)
        {
            _currentImageIndex -= 1;
            _image.sprite = _storyImages[_currentImageIndex];
        }
    }

    public void Skip()
    {
        SceneManager.LoadScene(1);
    }
}

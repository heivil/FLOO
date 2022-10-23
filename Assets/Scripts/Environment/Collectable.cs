using UnityEngine;
using TMPro;

public class Collectable : MonoBehaviour
{
    public Sprite[] _sprites;
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    public int _spriteAmount = 32;
    private Timer _timer;
    public int _amountCollected;
    public GameObject _particle;
    private AudioSource _audioSource;
    public AudioClip[] _sounds = new AudioClip[3];
    private TMP_Text _playerGreenChange;
    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _timer = gameObject.AddComponent<Timer>();
        _timer.OnTimerCompleted += DoneFeedbacking;
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.clip = _sounds[Random.Range(0, _sounds.Length)];
        if (_sprites != null)
        {
            int random = Random.Range(0, _spriteAmount);
            _renderer.sprite = _sprites[random];
        }
        _playerGreenChange = GameManager.Instance.LevelManager._player._greenChange;
        if (GameManager.Instance.MoreGreen)
        {
            _amountCollected = _amountCollected * 2;
        }
    }

    private void OnDestroy()
    {
        _timer.OnTimerCompleted -= DoneFeedbacking;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {  
            if(collision.gameObject.GetComponent<ICollect>() != null)
            {
                _collider.enabled = false;
                collision.gameObject.GetComponent<ICollect>().Collect(_amountCollected);
                _renderer.gameObject.SetActive(false);
                _particle.gameObject.SetActive(true);
                _audioSource.Play();
                _timer.StartTimer(1);
                _playerGreenChange.gameObject.SetActive(false);
                _playerGreenChange.text = _amountCollected.ToString();
                _playerGreenChange.color = GameManager.Instance.LevelManager._player._green;
                _playerGreenChange.gameObject.SetActive(true);
            }
        }
    }
    private void DoneFeedbacking() 
    {
        gameObject.SetActive(false);
    }
}

using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Goo _goo;
    public Flame _flame;
    //public int _collectedGreens;
    //[HideInInspector]
    public GameObject _activeUnit;
    public CameraScript _camera;
    public GameObject _greenCloud;
    public bool _snakeDoNotSwitchTargetLol;
    private Vector3 _flameSpawnLift = new Vector3(0, 0.25f, 0);
    public bool _alive = true;
    public TMP_Text _greenChange;
    public Color _green, _red, _invisible;
    [HideInInspector]
    public Timer _immortalityTimer;

    public bool Alive
    {
        get { return _alive; }
        set { _alive = value; }
    }
    private void Awake()
    {
        _immortalityTimer = gameObject.AddComponent<Timer>();
        _activeUnit = _goo.gameObject;
        _goo.gameObject.SetActive(true);
        _flame.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _greenChange.color = _invisible;
        _greenChange.gameObject.SetActive(true);
    }

    /// <summary>
    /// Swithces between the two forms of the player character
    /// </summary>
    public void SwitchUnit()
    {
        if (_alive)
        {
            _snakeDoNotSwitchTargetLol = true;
            transform.parent = null;
            _greenCloud.SetActive(false);
            if (_activeUnit.gameObject.GetComponent<Goo>() != null)
            {

                _flame.gameObject.SetActive(true);
                _activeUnit = _flame.gameObject;
                if (_activeUnit.GetComponent<Flame>() != null)
                {
                    _activeUnit.GetComponent<Flame>()._rB.velocity = _goo.GetComponent<Goo>()._rB.velocity;
                }
                _goo.gameObject.SetActive(false);
                _activeUnit.gameObject.transform.position = _goo.gameObject.transform.position + _flameSpawnLift;
                _activeUnit.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (_activeUnit.gameObject.GetComponent<Flame>() != null)
            {

                _goo.gameObject.SetActive(true);
                _activeUnit = _goo.gameObject;
                if (_activeUnit.GetComponent<Goo>() != null)
                {
                    _activeUnit.GetComponent<Goo>()._rB.velocity = _flame.GetComponent<Flame>()._rB.velocity;
                }
                _flame.gameObject.SetActive(false);
                _activeUnit.gameObject.transform.position = _flame.gameObject.transform.position;
                _activeUnit.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            GameManager.Instance.LevelManager._camera._followUnit = _activeUnit.gameObject;
            _greenCloud.transform.position = _activeUnit.transform.position;
            _greenCloud.SetActive(true);
            _snakeDoNotSwitchTargetLol = false;
        }
    }
}
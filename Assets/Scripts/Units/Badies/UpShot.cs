using UnityEngine;

public class UpShot : MonoBehaviour
{
    private Timer _lerpTimer;
    private Vector2 _startSize;
    public Vector2 _startPos, _targetPosition, _targetSize; 
    public float _shotTime = 2, _targetDistance = 2;
    private CapsuleCollider2D _collider;
    private UpShooter _shooter;

    private void Awake()
    {
        _lerpTimer = gameObject.AddComponent<Timer>();
        _startPos = transform.position;
        _targetPosition = _startPos + new Vector2(0 , _targetDistance);
        _collider = GetComponent<CapsuleCollider2D>();
        _startSize = _collider.size;
        _shooter = GetComponentInParent<UpShooter>();
        _lerpTimer.OnTimerCompleted += BeGone;
    }
    private void OnEnable()
    {
        transform.position = _startPos;
        transform.rotation = _shooter.transform.rotation;
        _lerpTimer.StartTimer(_shotTime);
    }

    private void OnDestroy()
    {
        _lerpTimer.OnTimerCompleted -= BeGone;
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(_startPos, _targetPosition, _lerpTimer.NormalizedTimeElapsed);
        _collider.size = Vector2.Lerp(_startSize, _targetSize, _lerpTimer.NormalizedTimeElapsed);
    }

    private void BeGone()
    {
        _collider.size = _startSize;
        gameObject.SetActive(false);
        _shooter._shootTimer.StartTimer(_shooter._shotInterval);
    }
}

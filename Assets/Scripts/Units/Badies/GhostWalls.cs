using UnityEngine;

public class GhostWalls : MonoBehaviour
{
    public float _transLerpTime = 1;
    private Ghost _ghost;
    private Timer _transTimer;
    private float _alpha = 1, _startLerp, _endLerp, _maxAlpha = 1.0f, _minAlpha = 0.5f;
    private SpriteRenderer _ghostRenderer;
    public float _wallDetectRadius = 0.25f, _enemyDetectRadius = 0.5f;
    int _groundLayerMask = 1 << 10;
    private Collider2D _ghostCollider;

    private void Awake()
    {
        _ghost = GetComponentInParent<Ghost>();
        _transTimer = gameObject.AddComponent<Timer>();
        _ghostRenderer = _ghost.GetComponentInChildren<SpriteRenderer>();
        _transTimer.OnTimerCompleted += ClearLerp;
        _ghostCollider = gameObject.GetComponentInParent<Collider2D>();
    }

    private void OnDestroy()
    {
        _transTimer.OnTimerCompleted -= ClearLerp;
    }
    private void Update()
    {
        DetectWalls();
        if (_transTimer._isRunning)
        {
            _alpha = Mathf.Lerp(_startLerp, _endLerp, _transTimer.NormalizedTimeElapsed);
            _ghostRenderer.color = new Color(1f, 1f, 1f, _alpha);
        }
    }

    /// <summary>
    /// Detects walls so ghost goes through them and turns transparent
    /// </summary>
    private void DetectWalls()
    {
        RaycastHit2D wallhit = Physics2D.CircleCast(transform.position, _wallDetectRadius, Vector2.zero, 0, _groundLayerMask);

        if (wallhit.collider != null)
        {
            if (_transTimer._isRunning == false && _alpha != _minAlpha)
            {
                _transTimer.StartTimer(_transLerpTime);
                _startLerp = _alpha;
                _endLerp = _minAlpha;
            }

        } else
        {
            if (_transTimer._isRunning == false && _alpha != _maxAlpha)
            {
                _transTimer.StartTimer(_transLerpTime);
                _startLerp = _alpha;
                _endLerp = 1f;
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Physics2D.IgnoreCollision(collision, _ghost._collider);
        }
    }

    private void ClearLerp()
    {
        _alpha = _endLerp;
        _startLerp = _alpha;
        
    }

}

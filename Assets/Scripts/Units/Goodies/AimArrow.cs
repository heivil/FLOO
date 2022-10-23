using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArrow : MonoBehaviour
{
    private SpriteRenderer _lineRenderer, _pointerRenderer;
    private GameObject _pointer;
    public Color32 _light = new Color32(51, 101, 176, 255), _medium = new Color32(33, 176, 202, 255), _dark = new Color32(18, 236, 255, 255);
    private float _ogLength;

    private void Awake()
    {
        _lineRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        _pointerRenderer = gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        _pointer = gameObject.transform.GetChild(1).gameObject;
        _ogLength = _lineRenderer.size.y;
    }

    public void Aim(float angle, float distance, float maxDist)
    {
        if(distance > maxDist)
        {
            distance = maxDist;
        }
        float normalizedDist = distance / maxDist;
        
        _lineRenderer.size = new Vector2(_lineRenderer.size.x, _ogLength * normalizedDist);
        _pointer.transform.localPosition = new Vector2(0, _lineRenderer.size.y + 0.75f);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (normalizedDist < 0.33f)
        {
            _lineRenderer.color = _dark;
            _pointerRenderer.color = _dark;
        }
        else if(normalizedDist > 0.33f &&  normalizedDist < 0.66f)
        {
            _lineRenderer.color = _medium;
            _pointerRenderer.color = _medium;
        }
        else if (normalizedDist > 0.66f)
        {
            _lineRenderer.color = _light;
            _pointerRenderer.color = _light;
        }
    }
}

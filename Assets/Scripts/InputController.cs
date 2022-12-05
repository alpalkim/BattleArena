using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private readonly float _holdThreshold = 0.5f;
    private bool _isHoldCompleted;
    private float _timer;
    private Button _button;
    
    private Coroutine _holdRoutine;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _holdRoutine = StartCoroutine(nameof(HoldTimerRoutine));
        //Position of the click
        Vector2 clickPosition = eventData.position;
        //Get the click position relative to screen
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(clickPosition.x, clickPosition.y, -Camera.main.transform.position.z));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // if (!_isHoldCompleted) SelectHero();
        Stop();
    }
    
    private IEnumerator HoldTimerRoutine()
    {
        _isHoldCompleted = false;
        while (true)
        {
            _timer += Time.deltaTime;
            if (_timer > _holdThreshold)
            {
                _isHoldCompleted = true;
                // OpenInfoPopUp();
                yield break;
            }
            yield return null;
        }
    }

    private void Stop()
    {
        if(_holdRoutine != null) StopCoroutine(_holdRoutine);
        _isHoldCompleted = false;
    }

}
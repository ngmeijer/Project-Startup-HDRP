using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InGameButtonClickable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Not accept clicks before duration (in secs)")]
    public float lockDuration;
    private bool _waitLockDuration;
    
    public PointerEventDataUnityEvent notifyOnClick;
    
    [Header("OnHover Events")] public UnityEvent notifyOnEnter;
    public UnityEvent notifyOnExit;

    private void Start()
    {
        _waitLockDuration = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_waitLockDuration)
            return;
        
        notifyOnClick?.Invoke(eventData);

        if (lockDuration > 0)
        {
            _waitLockDuration = true;
            StartCoroutine(WaitDurationRoutine());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        notifyOnEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        notifyOnExit?.Invoke();
    }
    
    private IEnumerator WaitDurationRoutine()
    {
        yield return new WaitForSeconds(lockDuration);
        _waitLockDuration = false;
    }
}
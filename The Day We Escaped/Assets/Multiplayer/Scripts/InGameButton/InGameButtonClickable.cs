using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InGameButtonClickable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PointerEventDataUnityEvent notifyOnClick;
    [Header("OnHover Events")] public UnityEvent notifyOnEnter;
    public UnityEvent notifyOnExit;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        notifyOnClick?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        notifyOnEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        notifyOnExit?.Invoke();

    }
}
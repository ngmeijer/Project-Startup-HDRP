using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class PointerEventDataAndIntUnityEvent : UnityEvent<PointerEventData, int>
{
}
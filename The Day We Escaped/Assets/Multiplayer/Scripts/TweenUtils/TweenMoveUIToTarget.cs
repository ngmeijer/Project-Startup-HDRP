using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using UnityEngine.Events;

public class TweenMoveUIToTarget : MonoBehaviour
{
    public RectTransform target;
    public RectTransform toTarget;
    public float duration = 0.4f;
    public float delay = 0;
    public bool playOnStart = false;
    public Tween.LoopType loopType = Tween.LoopType.None;
    public AnimationCurve easing = Tween.EaseInOut;
    public UnityEvent startCallback;
    public UnityEvent completeCallback;
    public UnityEvent startReverseCallback;
    public UnityEvent completeReverseCallback;

    private TweenBase _tween;
    
    private void Start()
    {
        if (target == null)
            target = this.GetComponent<RectTransform>();

        if (playOnStart)
        {
            Play();
        }
    }
    
    public void Play()
    {
        if (!enabled || toTarget == null || target == null)
        {
            Debug.LogWarning($"{this.gameObject}: target or toTarget null");
            return;
        }

        var fromPosition = target.anchoredPosition;
        var toPosition =  toTarget.anchoredPosition;
        
        _tween = Tween.AnchoredPosition(target, fromPosition, toPosition, duration, delay, easing,
            loopType,
            delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
    }

    public void PlayReverse()
    {
        if (!enabled || toTarget == null || target == null)
        {
            Debug.LogWarning($"{this.gameObject}: target or toTarget null");
            return;
        }

        var fromPosition = target.anchoredPosition;
        var toPosition =  toTarget.anchoredPosition;

        _tween = Tween.AnchoredPosition(target, toPosition, fromPosition, duration, delay,
            easing,
            loopType,
            delegate { startReverseCallback?.Invoke(); }, delegate { completeReverseCallback?.Invoke(); });
    }

    public void PlayToTarget(RectTransform t)
    {
        SetToTarget(t);
        Play();
    }
    
    public void SetToTarget(RectTransform t)
    {
        toTarget = t;
    }

    private void OnEnable()
    {
        Play();
    }
}
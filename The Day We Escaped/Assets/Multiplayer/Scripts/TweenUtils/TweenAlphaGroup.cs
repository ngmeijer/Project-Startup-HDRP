using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using UnityEngine.Events;

public class TweenAlphaGroup : MonoBehaviour
{
    public CanvasGroup target;
    public float fromAlpha = 0f;
    public float toAlpha = 1f;
    public bool forceFrom;
    public float duration = 0.4f;
    public float delay = 0;
    public bool playOnStart = false;
    public bool forceBeforeDelay;
    public Tween.LoopType loopType = Tween.LoopType.None;
    public AnimationCurve easing = Tween.EaseInOut;
    public UnityEvent startCallback;
    public UnityEvent completeCallback;

    private TweenBase _tween;

    private void Start()
    {
        if (target == null)
        {
            target = GetComponent<CanvasGroup>();
        }

        if (playOnStart)
        {
            Play();
        }
    }

    public void Play()
    {
        if (forceFrom)
        {
            if (forceBeforeDelay)
            {
                target.alpha = fromAlpha;
            }

            _tween = Tween.CanvasGroupAlpha(target, fromAlpha, toAlpha, duration, delay, easing,
                loopType, delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
        else
        {
            _tween = Tween.CanvasGroupAlpha(target, toAlpha, duration, delay, easing, loopType,
                delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
    }

    public void PlayReverse()
    {
        if (forceFrom)
        {
            if (forceBeforeDelay)
            {
                target.alpha = toAlpha;
            }
            
            _tween = Tween.CanvasGroupAlpha(target, toAlpha, fromAlpha, duration, delay, easing,
                loopType, delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
        else
        {
            _tween = Tween.CanvasGroupAlpha(target, fromAlpha, duration, delay, easing, loopType,
                delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
    }
}
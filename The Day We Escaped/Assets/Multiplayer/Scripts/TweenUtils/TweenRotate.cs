using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using UnityEngine.Events;

public class TweenRotate : MonoBehaviour
{
    public Transform target;
    public Vector3 fromRotation = Vector3.up * -60f;
    public Vector3 toRotation = Vector3.up * 60f;
    public Space space = Space.Self;
    public bool forceFromRotation;
    public float duration = 0.4f;
    public float delay = 0;
    public bool playOnStart = false;
    public Tween.LoopType loopType = Tween.LoopType.None;
    public AnimationCurve easing = Tween.EaseInOut;
    public UnityEvent startCallback;
    public UnityEvent completeCallback;

    private TweenBase _tween;

    private void Start()
    {
        if (target == null)
        {
            target = this.transform;
        }
        
        if (playOnStart)
        {
            Play();
        }
    }

    public void Play()
    {
        if (forceFromRotation)
        {
            if (space == Space.Self)
                _tween = Tween.LocalRotation(target, fromRotation, toRotation, duration, delay, easing,
                    loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
            else
                _tween = Tween.Rotation(target, fromRotation, toRotation, duration, delay, easing, loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
        else
        {
            if (space == Space.Self)
                _tween = Tween.LocalRotation(target, toRotation, duration, delay, easing, loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
            else
                _tween = Tween.Rotation(target, toRotation, duration, delay, easing, loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
    }

    public void PlayReverse()
    {
        if (forceFromRotation)
        {
            if (space == Space.Self)
                _tween = Tween.LocalRotation(target, toRotation, fromRotation, duration, delay, easing,
                    loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
            else
                _tween = Tween.Rotation(target, toRotation, fromRotation, duration, delay, easing, loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
        else
        {
            if (space == Space.Self)
                _tween = Tween.LocalRotation(target, fromRotation, duration, delay, easing, loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
            else
                _tween = Tween.Rotation(target, fromRotation, duration, delay, easing, loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
    }
}
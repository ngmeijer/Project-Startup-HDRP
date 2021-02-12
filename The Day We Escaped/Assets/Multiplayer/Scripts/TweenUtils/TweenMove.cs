using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using UnityEngine.Events;

public class TweenMove : MonoBehaviour
{
    public Transform target;
    public Vector3 fromPosition = Vector3.zero;
    public Vector3 toPosition = Vector3.forward;
    public Space space = Space.Self;
    public bool addMove;
    public bool forceFromPosition;
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
            target = this.transform;

        if (playOnStart)
        {
            Play();
        }
    }

    public void Play()
    {
        Vector3 startPos;

        if (forceFromPosition)
        {
            if (space == Space.Self)
            {
                startPos = addMove ? target.localPosition : Vector3.zero;

                _tween = Tween.LocalPosition(target, startPos + fromPosition, startPos + toPosition, duration, delay,
                    easing,
                    loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
            }
            else
            {
                startPos = addMove ? target.position : Vector3.zero;

                _tween = Tween.Position(target, startPos + fromPosition, startPos + toPosition, duration, delay, easing,
                    loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
            }
        }
        else
        {
            if (space == Space.Self)
            {
                startPos = addMove ? target.localPosition : Vector3.zero;

                _tween = Tween.LocalPosition(target, startPos + toPosition, duration, delay, easing, loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
            }
            else
            {
                startPos = addMove ? target.position : Vector3.zero;

                _tween = Tween.Position(target, startPos + toPosition, duration, delay, easing, loopType,
                    delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
            }
        }
    }

    public void PlayReverse()
    {
        Vector3 startPos = Vector3.zero;

        if (forceFromPosition)
        {
            if (space == Space.Self)
            {
                startPos = addMove ? target.localPosition : Vector3.zero;

                _tween = Tween.LocalPosition(target, toPosition - startPos, fromPosition - startPos, duration, delay,
                    easing,
                    loopType,
                    delegate { startReverseCallback?.Invoke(); }, delegate { completeReverseCallback?.Invoke(); });
            }
            else
            {
                startPos = addMove ? target.position : Vector3.zero;

                _tween = Tween.Position(target, toPosition - startPos, fromPosition - startPos, duration, delay, easing,
                    loopType,
                    delegate { startReverseCallback?.Invoke(); }, delegate { completeReverseCallback?.Invoke(); });
            }
        }
        else
        {
            if (space == Space.Self)
            {
                var newFromPos = fromPosition;
                if (addMove)
                {
                    startPos = toPosition;
                    newFromPos = target.localPosition;
                }

                _tween = Tween.LocalPosition(target, newFromPos - startPos, duration, delay, easing, loopType,
                    delegate { startReverseCallback?.Invoke(); }, delegate { completeReverseCallback?.Invoke(); });
            }
            else
            {
                var newFromPos = fromPosition;
                if (addMove)
                {
                    startPos = toPosition;
                    newFromPos = target.position;
                }

                _tween = Tween.Position(target, newFromPos - startPos, duration, delay, easing, loopType,
                    delegate { startReverseCallback?.Invoke(); }, delegate { completeReverseCallback?.Invoke(); });
            }
        }
    }
}
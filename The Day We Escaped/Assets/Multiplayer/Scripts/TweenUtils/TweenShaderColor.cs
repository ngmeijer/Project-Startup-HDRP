using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using UnityEngine.Events;

public class TweenShaderColor : MonoBehaviour
{
    public string colorName = "_Color";
    public Color fromColor = Color.black;
    public Color toColor = Color.white;
    public bool forceFromColor;
    public float duration = 0.4f;
    public float delay = 0;
    public bool playOnStart = false;
    public Tween.LoopType loopType = Tween.LoopType.None;
    public AnimationCurve easing = Tween.EaseInOut;
    public Renderer rend;
    public UnityEvent startCallback;
    public UnityEvent completeCallback;

    private TweenBase _tween;
    
    private void Start()
    {
        if (rend == null)
            rend = GetComponent<Renderer>();

        if (playOnStart)
        {
            Play();
        }
    }

    public void Play()
    {
        if (forceFromColor)
        {
            _tween = Tween.ShaderColor(rend.material, colorName, fromColor, toColor, duration, delay, easing, loopType,
                delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
        else
        {
            _tween = Tween.ShaderColor(rend.material, colorName, toColor, duration, delay, easing, loopType,
                delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
    }
    
    public void PlayReverse()
    {
        if (forceFromColor)
        {
            _tween = Tween.ShaderColor(rend.material, colorName, toColor, fromColor, duration, delay, easing, loopType,
                delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
        else
        {
            _tween = Tween.ShaderColor(rend.material, colorName, fromColor, duration, delay, easing, loopType,
                delegate { startCallback?.Invoke(); }, delegate { completeCallback?.Invoke(); });
        }
    }
}
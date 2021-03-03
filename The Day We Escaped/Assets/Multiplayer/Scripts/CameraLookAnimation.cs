using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraLookAnimation : MonoBehaviour
{
    [SerializeField] private float _transitionTime = 2f;

    [SerializeField] private Spline _spline;

    public bool playOnStart;

    public UnityEvent notifyOnStart;
    public UnityEvent notifyOnComplete;
    
    public UnityEvent notifyOnStartReverse;
    public UnityEvent notifyOnCompleteReverse;

    
    private void Start()
    {
        if (playOnStart)
            Play();
    }

    public void Play()
    {
        Tween.Value(_spline.followers[0].percentage, 1, delegate(float value) { _spline.followers[0].percentage = value; }, _transitionTime, 0,
            Tween.EaseInOut, Tween.LoopType.None, delegate { notifyOnStart?.Invoke(); }, delegate { notifyOnComplete?.Invoke(); });
    }
    
    public void PlayReverse()
    {
        Tween.Value(_spline.followers[0].percentage, 0, delegate(float value) { _spline.followers[0].percentage = value; }, _transitionTime, 0,
            Tween.EaseInOut, Tween.LoopType.None, delegate { notifyOnStartReverse?.Invoke(); }, delegate { notifyOnCompleteReverse?.Invoke(); });
    }
}
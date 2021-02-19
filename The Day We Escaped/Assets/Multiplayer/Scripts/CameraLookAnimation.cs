using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAnimation : MonoBehaviour
{
    [SerializeField] private float _transitionTime = 2f;

    [SerializeField] private Spline _spline;


    private void Start()
    {
        Tween.Value(0, 1, delegate (float value)
        {
            _spline.followers[0].percentage = value;
        }, _transitionTime, 0, Tween.EaseInOut, Tween.LoopType.None);
    }
}

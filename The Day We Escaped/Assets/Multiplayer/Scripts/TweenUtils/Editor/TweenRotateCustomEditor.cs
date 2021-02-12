using Pixelplacement;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TweenRotate))]
public class TweenRotateCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Play"))
        {
            var tween = (TweenRotate) target;
            tween.Play();
        }
        else if (GUILayout.Button("Play Reverse"))
        {
            var tween = (TweenRotate) target;
            tween.PlayReverse();
        }
    }
}
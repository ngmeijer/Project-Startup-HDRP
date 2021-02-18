using Pixelplacement;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TweenAlphaGroup))]
public class TweenAlphaGroupCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Play"))
        {
            var tween = (TweenAlphaGroup) target;
            tween.Play();
        }
        else if (GUILayout.Button("Play Reverse"))
        {
            var tween = (TweenAlphaGroup) target;
            tween.PlayReverse();
        }
    }
}
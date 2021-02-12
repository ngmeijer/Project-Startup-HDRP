using Pixelplacement;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TweenMove))]
public class TweenMoveCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Play"))
        {
            var tween = (TweenMove) target;
            tween.Play();
        }
        else if (GUILayout.Button("Play Reverse"))
        {
            var tween = (TweenMove) target;
            tween.PlayReverse();
        }
    }
}
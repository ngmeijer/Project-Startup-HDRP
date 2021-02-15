using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleIntStateBehaviour))]
public class SimpleIntStateBehaviourCustomEditor : Editor
{
    private int _stateNumber;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        EditorGUILayout.LabelField("For Test");
        
        if (GUILayout.Button("Next State In Server"))
        {
            var ss = target as SimpleIntStateBehaviour;
            if (ss != null)
            {
                ss.NextStateInServer();
            }
        }

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set State"))
        {
            var ss = target as SimpleIntStateBehaviour;
            if (ss != null)
            {
                ss.SetStateInServer(_stateNumber);
            }
        }

        _stateNumber = EditorGUILayout.IntField(_stateNumber);
        EditorGUILayout.EndHorizontal();
    }
}
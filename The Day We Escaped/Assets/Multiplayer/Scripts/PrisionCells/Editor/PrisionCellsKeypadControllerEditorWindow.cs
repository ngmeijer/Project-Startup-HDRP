using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

public class PrisionCellsKeypadControllerEditorWindow : EditorWindow
{
    private GameObject keyPadGameObject;

    [MenuItem("GameTools/Config KeyPad Buttons Events")]
    private static void ShowWindow()
    {
        var window = GetWindow<PrisionCellsKeypadControllerEditorWindow>();
        window.titleContent = new GUIContent("Config KeyPad Buttons Events");
        window.Show();
    }

    private void OnGUI()
    {
        keyPadGameObject =
            EditorGUILayout.ObjectField("Keypad Controller", keyPadGameObject, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Config"))
        {
            ConfigKeyPad();
        }
    }

    private void ConfigKeyPad()
    {
        var buttons = keyPadGameObject.GetComponentsInChildren<InGameSimpleButtonBoltCallback>();

        foreach (var button in buttons)
        {
            CleanPersistentListeners(button.onEventReceived);
            CleanPersistentListeners(button.onEventReceivedInServer);

            var tweenMove = button.GetComponent<TweenMove>();
            
            UnityEventTools.AddPersistentListener(button.onEventReceived, tweenMove.Play);
            UnityEventTools.AddPersistentListener(button.onEventReceivedInServer, tweenMove.Play);

            var inGameInputReceiver = keyPadGameObject.GetComponent<InGameInputReceiver>();
            
            UnityEventTools.AddStringPersistentListener(button.onEventReceived, inGameInputReceiver.ReceiveInput, button.Id.ToString());
            UnityEventTools.AddStringPersistentListener(button.onEventReceivedInServer, inGameInputReceiver.ReceiveInput, button.Id.ToString());
            
            EditorUtility.SetDirty(button);
        }
    }

    void CleanPersistentListeners(UnityEvent evt)
    {
        for (int i = evt.GetPersistentEventCount() - 1; i >= 0; i--)
        {
            UnityEventTools.RemovePersistentListener(evt, i);
        }
    }
}
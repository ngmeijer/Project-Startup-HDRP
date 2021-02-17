using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Multiplayer.Scripts.Editor
{
    public class RemoveMeshFilterAndMeshRenderWindowEditor : EditorWindow
    {
        private Transform _parent;
        
        [MenuItem("Tools/Remove MeshFilter And MeshRender")]
        private static void ShowWindow()
        {
            var window = GetWindow<RemoveMeshFilterAndMeshRenderWindowEditor>();
            window.titleContent = new GUIContent("Remove MeshFilter And MeshRender");
            window.Show();
        }

        private void OnGUI()
        {
            _parent = EditorGUILayout.ObjectField("Parent", _parent, typeof(Transform), true) as Transform;
            
            if (GUILayout.Button("Remove"))
            {
                if (_parent != null)
                {
                    //RemovedComponentInChildren<ProBuilderMesh>(_parent);
                    //RemovedComponentInChildren<PolyShape>(_parent);
                    RemovedComponentInChildren<MeshRenderer>(_parent);
                }
            }
        }

        private void RemovedComponentInChildren<T>(Transform pParent) where T : Component
        {
            var components = pParent.GetComponentsInChildren<T>();

            for (int i = 0; i < components.Length; i++)
            {
                var c = components[i];
                
                DestroyImmediate(c);
            }
            
            EditorUtility.SetDirty(pParent);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


    public class CheckBoltEntityIdsInSceneWindowEditor : EditorWindow
    {
        [MenuItem("Tools/Check BoltEntity Ids In Scene")]
        private static void ShowWindow()
        {
            var window = GetWindow<CheckBoltEntityIdsInSceneWindowEditor>();
            window.titleContent = new GUIContent("Check BoltEntity Ids In Scene");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Check"))
            {
                var boltEntities = GetAllBoltEntitiesInScene();

                //Group by sceneId
                var groupBy = boltEntities.Select(b => b.ModifySettings()).GroupBy(b => b.sceneId);

                var report = "sceneId\t|\tCount\r\n";
                
                foreach (var g in groupBy)
                {
                    int c = g.Count();
                    
                    if (c > 1)
                    {
                        report += $"{g.Key}\t|\t{g.Count()}";
                    }
                }
                
                Debug.Log(report);
            }
            
            EditorGUILayout.LabelField("Look the reports in console");
        }
        
        List<BoltEntity> GetAllBoltEntitiesInScene()
        {
            List<BoltEntity> boltEntitiesInScene = new List<BoltEntity>();

            foreach (BoltEntity go in Resources.FindObjectsOfTypeAll(typeof(BoltEntity)) as BoltEntity[])
            {
                if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                    boltEntitiesInScene.Add(go);
            }

            return boltEntitiesInScene;
        }
    }

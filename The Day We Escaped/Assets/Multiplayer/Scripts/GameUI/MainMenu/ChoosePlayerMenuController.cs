using System;
using System.Collections;
using Bolt.Matchmaking;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class ChoosePlayerMenuController : MonoBehaviour
    {
        public int sceneIndex;

        [SerializeField] private Text _text;

        public UnityEvent notifySceneLoading;
        
        /// <summary>
        /// Wait X seconds than load the scene, runs only in server
        /// </summary>
        /// <param name="playerType"></param>
        public void LoadPlayerScene(int playerType)
        {
            StartCoroutine(LoadPlayerSceneRoutine(playerType));
        }

        private IEnumerator LoadPlayerSceneRoutine(int serverPlayerType)
        {
            notifySceneLoading?.Invoke();
            
            yield return new WaitForSeconds(3f);

            foreach (var entity in BoltNetwork.Entities)
            {
                BoltNetwork.Destroy(entity);
            }
            
            var serverPlayerToken = new IntBoltToken()
            {
                intVal = serverPlayerType
            };

            var scene = NameFromIndex(sceneIndex);

            //Load sceneName and sends the token with the player type
            BoltNetwork.LoadScene(scene, serverPlayerToken);
        }
        
        private static string NameFromIndex(int BuildIndex)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
            int slash = path.LastIndexOf('/');
            string name = path.Substring(slash + 1);
            int dot = name.LastIndexOf('.');
            return name.Substring(0, dot);
        }
    }
}
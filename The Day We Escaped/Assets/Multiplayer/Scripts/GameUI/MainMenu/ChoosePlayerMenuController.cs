using System;
using System.Collections;
using Bolt.Matchmaking;
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
        
        private void Start()
        {
            if (!BoltNetwork.IsServer)
            {
                _text.text = "Server Player is choosing the Player number";
            }
        }

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

            var scene = SceneManager.GetSceneByBuildIndex(sceneIndex);
            
            //Load sceneName and sends the token with the player type
            BoltNetwork.LoadScene(scene.name, serverPlayerToken);
        }
    }
}
using System;
using System.Collections;
using Bolt.Matchmaking;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class ChoosePlayerMenuController : MonoBehaviour
    {
        public string sceneName;

        [SerializeField] private Text _text;

        private void Start()
        {
            _text.gameObject.SetActive(!BoltNetwork.IsServer);
        }

        /// <summary>
        /// Wait X seconds than load the scene
        /// </summary>
        /// <param name="playerType"></param>
        public void LoadPlayerScene(int playerType)
        {
            StartCoroutine(LoadPlayerSceneRoutine(playerType));
        }

        private IEnumerator LoadPlayerSceneRoutine(int serverPlayerType)
        {
            yield return new WaitForSeconds(3f);

            foreach (var entity in BoltNetwork.Entities)
            {
                BoltNetwork.Destroy(entity);
            }
            
            var serverPlayerToken = new IntBoltToken()
            {
                intVal = serverPlayerType
            };

            //Load sceneName and sends the token with the player type
            BoltNetwork.LoadScene(sceneName, serverPlayerToken);
        }
    }
}
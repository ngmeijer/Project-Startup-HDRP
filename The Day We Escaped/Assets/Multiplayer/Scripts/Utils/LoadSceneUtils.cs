using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneUtils : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }
    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}

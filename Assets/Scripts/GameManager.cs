using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

	
    public class GameManager : MonoBehaviour
{
    void Start()
    {
        if (Application.isEditor)
        {
            Scene loadedLevel = SceneManager.GetSceneByName("Level Forest1");
            if (loadedLevel.isLoaded)
            {
                SceneManager.SetActiveScene(loadedLevel);
                return;
            }
        }

        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        enabled = false;
        yield return SceneManager.LoadSceneAsync(
            "Level Forest1", LoadSceneMode.Additive
        );
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level Forest1"));
        enabled = true;
    }
}

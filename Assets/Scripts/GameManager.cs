using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

	
    public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    void Awake()
    {
        /*if (Application.isEditor)
        {*/
            Scene loadedLevel = SceneManager.GetSceneByName("Level Forest1");
            if (loadedLevel.isLoaded)
            {
                SceneManager.SetActiveScene(loadedLevel);
                GetComponent<DynamicGrid>().loadGrid();
                return;
            }
        //}
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
        GetComponent<DynamicGrid>().loadGrid();
        //DEBUG: Hard coded, need to create a dedicated script later
        GetComponent<DynamicGrid>().placeInGrid(Vector2.zero,player.gameObject);
    }
}

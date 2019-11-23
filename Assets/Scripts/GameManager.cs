using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

	
    public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private int loadedLevelBuildIndex = 0;
    void Start()
    {
        int index = 2;
        Scene loadedLevel = SceneManager.GetSceneByBuildIndex(index);
        if (loadedLevel.isLoaded)
        {
            SceneManager.SetActiveScene(loadedLevel);
            GetComponent<DynamicGrid>().loadObstacles();
            return;
        }
        else
            StartCoroutine(LoadLevel(index));
    }

    IEnumerator LoadLevel(int levelBuildIndex)
    {
        enabled = false;
        if (loadedLevelBuildIndex > 0 && loadedLevelBuildIndex != levelBuildIndex)
            yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);

        yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
        loadedLevelBuildIndex = levelBuildIndex;
        enabled = true;
        GetComponent<DynamicGrid>().loadObstacles();
        //DEBUG: Hard coded, need to create a dedicated script later
        //GetComponent<DynamicGrid>().placeInGrid(Vector2.zero,player.gameObject);
    }
}

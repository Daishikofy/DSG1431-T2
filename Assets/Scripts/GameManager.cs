using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

	
    public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Image transition;
    [SerializeField]
    private int firstSceneIndex = 2;

    private int loadedLevelBuildIndex = 0;
    void Start()
    {
        Scene loadedLevel = SceneManager.GetSceneByBuildIndex(firstSceneIndex);
        if (loadedLevel.isLoaded)
        {
            SceneManager.SetActiveScene(loadedLevel);
            GetComponent<DynamicGrid>().loadObstacles();
            loadedLevelBuildIndex = firstSceneIndex;
            return;
        }
        else
            StartCoroutine(LoadLevel(firstSceneIndex));
    }

    public void changeLevel(int levelIndex)
    {
        GetComponent<DynamicGrid>().clear();
        StartCoroutine(LoadLevel(levelIndex));
    }

    IEnumerator LoadLevel(int levelBuildIndex)
    {
        enableGame(false);

        Color color = transition.color;
        color.a = 1f;
        transition.color = color;

        if (loadedLevelBuildIndex > 0 && loadedLevelBuildIndex != levelBuildIndex)
            yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);

        yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
        loadedLevelBuildIndex = levelBuildIndex;


        while (transition.color.a > 0)
        {
            color = transition.color;
            color.a -= 0.1f;
            transition.color = color;
            yield return null;
        }

        enableGame(true);
        GetComponent<DynamicGrid>().loadObstacles();
        //DEBUG: Hard coded, need to create a dedicated script later
        //GetComponent<DynamicGrid>().placeInGrid(Vector2.zero,player.gameObject);
    }

    IEnumerator fadeOut()
    {
        while (transition.color.a > 0)
        {
            Color color = transition.color;
            color.a -= 0.1f;
            transition.color = color;
            yield return null;
        }
    }

    IEnumerator fadeIn()
    {
        while (transition.color.a < 0)
        {
            Color color = transition.color;
            color.a += 0.1f;
            transition.color = color;
            yield return null;
        }
    }
    private void enableGame(bool value)
    {
        /*if(value)
            StartCoroutine("fadeOut");
        else
            StartCoroutine("fadeIn");*/
        player.enabled = value;
        enabled = value;
    }

}

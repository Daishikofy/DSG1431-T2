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
    [SerializeField]
    private Image gameOverPanel;

    private int loadedLevelBuildIndex = 0;

    private CatsInput controller;
    private bool isPaused = false;
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
        gameOverPanel.gameObject.SetActive(false);
        player.Ko.AddListener(gameOver);

        controller = new CatsInput();
        controller.Player.Restart.Enable();
        controller.Player.Restart.performed += context => pauseGame();
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

        Debug.Log("loadedLevelBuildIndex: " + loadedLevelBuildIndex + " - levelBuildIndex: " + levelBuildIndex);

        if (loadedLevelBuildIndex > 0 && loadedLevelBuildIndex != levelBuildIndex)
         yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);

        if (loadedLevelBuildIndex != levelBuildIndex)
        { 
            yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
            loadedLevelBuildIndex = levelBuildIndex;
        }

        while (transition.color.a > 0)
        {
            color = transition.color;
            color.a -= 0.1f;
            transition.color = color;
            yield return null;
        }

        enableGame(true);
        GetComponent<DynamicGrid>().loadObstacles();
    }

    public IEnumerator fadeOut()
    {
        transition.color = Color.black;
        while (transition.color.a > 0)
        {
            Color color = transition.color;
            color.a -= 0.1f;
            transition.color = color;
            yield return null;
        }
    }

    public IEnumerator fadeIn()
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
        player.enabled = value;
        enabled = value;
    }

    private void gameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
    }

    public void resetGame()
    {
        changeLevel(firstSceneIndex);
        //TODO: Have a true variable for the player's position
        player.reset();
        gameOverPanel.gameObject.SetActive(false);
    }

    public void mainMenu()
    {
        enableGame(false);

        Color color = transition.color;
        color.a = 1f;
        transition.color = color;

        Scene mainMenu = SceneManager.GetSceneByName("MainMenu");
        SceneManager.LoadScene("MainMenu");
       // SceneManager.SetActiveScene(mainMenu);
    }

    private void pauseGame()
    {
        if (isPaused)
        {
            isPaused = false;      
            Time.timeScale = 1;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
        }
    }
}

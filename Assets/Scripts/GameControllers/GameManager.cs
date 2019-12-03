using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

	
    public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField]
    private Player player;
    [SerializeField]
    private Image transition;
    [SerializeField]
    private int firstSceneIndex = 2;
    [SerializeField]
    private Image gameOverPanel;
    [SerializeField]
    private UIController uiController;

    private int mainSceneIndex = 1;
    private int loadedLevelBuildIndex = 0;

    private CatsInput controller;
    private bool isPaused = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public static GameManager Instance { get{ return instance; } }
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

        //controller = new CatsInput();
        //uiController = FindObjectOfType<UIController>();
        if (uiController == null)
            Debug.Log("nao foi carregado");
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

        if (loadedLevelBuildIndex != mainSceneIndex && loadedLevelBuildIndex != levelBuildIndex)
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
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator fadeIn()
    {
        while (transition.color.a < 0)
        {
            Color color = transition.color;
            color.a += 0.1f;
            transition.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void enableGame(bool value)
    {
        player.enabled = value;
        enabled = value;
    }

    private void gameOver()
    {
        Debug.Log("Show gameOver");
        uiController.showGameOver();
    }

    public void resetGame()
    {
        if (firstSceneIndex == loadedLevelBuildIndex)
        {            
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            loadedLevelBuildIndex = mainSceneIndex;           
        }
        changeLevel(firstSceneIndex);
        //TODO: Have a true variable for the player's position   
        player.reset();
        if (uiController != null)
            uiController.showGameOver();
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

    public void pauseGame()
    {
        Debug.Log("PAUSE");
        if (isPaused)
        {//Unpaused
            isPaused = false;      
            //Time.timeScale = 1;
            player.enableController(true);
        }
        else
        {//Paused
            isPaused = true;
            //Time.timeScale = 0;
            player.enableController(false);
            player.stopForFrames(1);
        }
    }
}

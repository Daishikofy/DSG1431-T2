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
    private int firstSceneIndex = 3;
    [SerializeField]
    private Image gameOverPanel;
    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private AudioClip[] musics;
    private int mainSceneIndex = 1;
    private int loadedLevelBuildIndex = 1;

    private CatsInput controller;
    private bool isPaused = false;
    private bool isGameOver = false;
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

        DialogueManager.Instance.startDialogue.AddListener(pauseGame);
        DialogueManager.Instance.endedDialogue.AddListener(pauseGame);

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
        AudioManager.instance.PlayMusic(musics[levelBuildIndex - 2]);
        Color color = transition.color;
        color.a = 1f;
        transition.color = color;

        Debug.Log("loadedLevelBuildIndex: " + loadedLevelBuildIndex + " - levelBuildIndex: " + levelBuildIndex);

        if (loadedLevelBuildIndex != mainSceneIndex /*&& loadedLevelBuildIndex != levelBuildIndex*/)
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
        Debug.Log("Load over");
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
        while (transition.color.a < 1)
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
        isGameOver = true;
        uiController.showGameOver();
    }

    public void resetGame()
    {
 
        StartCoroutine(ressetingProcess());
    }

    private IEnumerator ressetingProcess()
    {
        yield return StartCoroutine(fadeIn());
        
        if (uiController != null && isGameOver)
        {
            uiController.showGameOver();
            isGameOver = false;
        }

        if (firstSceneIndex == loadedLevelBuildIndex)
        {
            //Debug.Log("1-firstSceneIndex" + firstSceneIndex);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            loadedLevelBuildIndex = mainSceneIndex;
        }
        //HORRIBLE CODE HERE

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        loadedLevelBuildIndex = mainSceneIndex;

        enableGame(false);
        AudioManager.instance.PlayMusic(musics[firstSceneIndex - 2]);
        Color color = transition.color;
        color.a = 1f;
        transition.color = color;

        yield return SceneManager.LoadSceneAsync(firstSceneIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(firstSceneIndex));
        loadedLevelBuildIndex = firstSceneIndex;
        

        Debug.Log("Player Reset");
        player.reset();

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    Image transition;
    [SerializeField]
    int firstSceneIndex;

    public void newGame()
    {
        //Audio: selected new game sound
        StartCoroutine(loadNewGame());
    }
    private IEnumerator loadNewGame()
    {
        Color color = transition.color;
        color.a = 1f;
        transition.color = color;

        yield return SceneManager.LoadSceneAsync(firstSceneIndex, LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(firstSceneIndex));
    }   

    public void exitGame()
    {
        Application.Quit();
    }

    public void showCredits()
    {
        //TODO
    }
}

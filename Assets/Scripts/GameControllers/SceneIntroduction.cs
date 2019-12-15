using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneIntroduction : MonoBehaviour
{
    [SerializeField]
    Image transition;
    [SerializeField]
    GameObject dialoguePanel;

    [SerializeField]
    Dialogue dialogue;
    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.Instance.endedDialogue.AddListener(dialogueEnded);
        StartCoroutine(introduction());
    }

    private IEnumerator introduction()
    {
        transition.color = Color.black;
        while (transition.color.a > 0)
        {
            Color color = transition.color;
            color.a -= 0.1f;
            transition.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        dialoguePanel.GetComponent<Animator>().SetBool("IsOpen", true);
        DialogueManager.Instance.StartDialogue(dialogue);
        
    }

    private void dialogueEnded()
    {
        StartCoroutine(fadeIn());
    }

    public IEnumerator fadeIn()
    {
        transition.color = Color.black;
        while (transition.color.a < 1)
        {
            Color color = transition.color;
            color.a += 0.1f;
            transition.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        StartCoroutine(loadNewGame());
    }
    private IEnumerator loadNewGame()
    {
        Color color = transition.color;
        color.a = 1f;
        transition.color = color;

        yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
    }
}

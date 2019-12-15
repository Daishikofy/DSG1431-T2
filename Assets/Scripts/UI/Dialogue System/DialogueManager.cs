using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    private Queue<string> sentences;
    private Animator animator;

    [SerializeField]
    private TextMeshProUGUI name;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Image dialogueBox;
    [SerializeField]
    private Button button;

    public UnityEvent startDialogue;
    public UnityEvent endedDialogue;

    private void Awake()
    {
        animator = dialogueBox.GetComponent<Animator>();
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            sentences = new Queue<string>();
        }
    }
    public static DialogueManager Instance
    {
        get
        {
            return instance;
        }
    }


    public void StartDialogue(Dialogue dialogue)
    {
        
        animator.SetBool("IsOpen", true);
        //GameManager.Instance.pauseGame();
        startDialogue.Invoke();

        EventSystem.current.SetSelectedGameObject(button.gameObject);
        sentences.Clear();
        name.text = dialogue.name;
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        displayNextSentence();
    }

    public void displayNextSentence()
    {
        //Audio sound - validate
        if (sentences.Count == 0)
        {
            endDialogue();
        }
        else
        {
            string sentence = sentences.Dequeue();
            StartCoroutine(displaySentence(sentence));
        }
        //TODO: UI controller to call nextsentence
    }

    private IEnumerator displaySentence(string sentence)
    {
        string displayed = "";
        foreach (var letter in sentence)
        {
            //Audio - Sound letter typing
            displayed += letter;
            text.text = displayed;
            yield return null;
        }   
    }

    private void endDialogue()
    {
        endedDialogue.Invoke();
        animator.SetBool("IsOpen", false);
        EventSystem.current.SetSelectedGameObject(null);
    }

}

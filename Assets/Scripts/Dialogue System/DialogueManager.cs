using System.Collections.Generic;
using UnityEngine;
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
        if (sentences.Count == 0)
        {
            endDialogue();
        }
        else
        {
            string sentence = sentences.Dequeue();
            text.text = sentence;
        }
        //TODO: UI controller to call nextsentence
    }

    private void endDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}

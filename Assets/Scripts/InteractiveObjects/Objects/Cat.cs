using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : SolidInteractiveObject
{
    CharacterManager manager;
    bool firstDialogue = false;
    [SerializeField][TextArea]
    string[] laterSentences;
    public override void onInteraction(Player player)
    {
        if (firstDialogue)
        {
            if (manager == null)
                manager = FindObjectOfType<CharacterManager>();
            StartCoroutine(changeBody(player));
        }
        else
        {
            base.onInteraction(player);
            firstDialogue = true;
        }
    }

    private IEnumerator changeBody(Player player)
    {
        yield return StartCoroutine(GameManager.Instance.fadeIn());
        manager.changeSkin("cat", player);
        dialogue.sentences = laterSentences;
        FindObjectOfType<DynamicGrid>().removeFromGrid(this.transform.position);
        GetComponent<SpriteRenderer>().enabled = false;

        yield return StartCoroutine(GameManager.Instance.fadeOut());
        base.onInteraction(player);
        
    }
}

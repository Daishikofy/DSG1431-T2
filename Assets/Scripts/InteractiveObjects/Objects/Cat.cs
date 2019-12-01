using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : SolidInteractiveObject
{
    CharacterManager manager;
    bool firstDialogue = false;
    [SerializeField][TextArea]
    string[] laterSentences;
    [SerializeField]
    [TextArea]
    string[] laterSentences2;
    [SerializeField]
    Collider2D exit;
    public override void onInteraction(Player player)
    {
        if (player.inventory.containsItem("HiddenKey"))
        {
            if (firstDialogue)
            {
                if (manager == null)
                    manager = FindObjectOfType<CharacterManager>();
                dialogue.sentences = laterSentences2;
                StartCoroutine(changeBody(player));
            }
            else
            {
                //TODO : No hard code allowed
                dialogue.sentences = laterSentences;
                dialogue.name = "Cassandra";
                base.onInteraction(player);
                firstDialogue = true;
            }
        }
        else
            base.onInteraction(player);
    }

    private IEnumerator changeBody(Player player)
    {
        yield return StartCoroutine(GameManager.Instance.fadeIn());
        manager.changeSkin("cat", player);
        FindObjectOfType<DynamicGrid>().removeFromGrid(this.transform.position);
        GetComponent<SpriteRenderer>().enabled = false;

        yield return StartCoroutine(GameManager.Instance.fadeOut());
        base.onInteraction(player);
        exit.enabled = true;
    }
}

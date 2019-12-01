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
            manager.changeSkin("cat", player);
            dialogue.sentences = laterSentences;
            base.onInteraction(player);

            FindObjectOfType<DynamicGrid>().removeFromGrid(this.transform.position);
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            base.onInteraction(player);
            firstDialogue = true;
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollectibleTalkingItem : MonoBehaviour
{
    [SerializeField]
    string itemName = "None";
    [SerializeField]
    Dialogue dialogue;
    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
            player.addToInventory(itemName);
            Destroy(this.gameObject);      
        }
    }
}
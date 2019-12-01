using UnityEngine;

/*
 * Object the player collects when clicking on it 
 * stored in the inventory
 */
public class SolidInteractiveObject : MonoBehaviour, InterfaceInteractiveObject
{
    [SerializeField]
    protected Dialogue dialogue;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        var grid = FindObjectOfType<DynamicGrid>();
        grid.placeInGrid((Vector2)transform.position, this.gameObject);
    }
    public virtual void onInteraction(Player player)
    {
       Debug.Log("Interaction with: " + gameObject.name);
        if (dialogue != null)
            DialogueManager.Instance.StartDialogue(dialogue);
    }
}
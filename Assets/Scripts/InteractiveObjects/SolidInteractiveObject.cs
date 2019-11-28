using UnityEngine;

/*
 * Object the player collects when clicking on it 
 * stored in the inventory
 */
public class SolidInteractiveObject : MonoBehaviour, InterfaceInteractiveObject
{
    // Start is called before the first frame update
    protected void Start()
    {
        Debug.Log("cadastrou: " + (Vector2)transform.position);
        var grid = FindObjectOfType<DynamicGrid>();
        grid.placeInGrid((Vector2)transform.position, this.gameObject);
    }
    public virtual void onInteraction(Player player)
    {
        Debug.Log("Solid object");
    }
}
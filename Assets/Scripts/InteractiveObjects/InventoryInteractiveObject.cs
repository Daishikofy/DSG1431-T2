using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Object the player collects when clicking on it 
 * stored in the inventory
 */
public class InventoryInteractiveObject : MonoBehaviour, InterfaceInteractiveObject
{
    // Start is called before the first frame update
    private void Start()
    {
        var grid = FindObjectOfType<DynamicGrid>();
		grid.placeInGrid((Vector2)transform.position, this.gameObject);
    }
    public void onInteraction(Player player)
	{
        Debug.Log("The player is interacting with me!! :D");
	}
}

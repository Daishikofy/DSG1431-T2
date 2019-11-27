using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteractiveObject : InterfaceInteractiveObject
{
    // Start is called before the first frame update
    protected override void Start()
    {
        var grid = FindObjectOfType<DynamicGrid>();
		grid.placeInGrid((Vector2)transform.position);
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
	
	public override void onInteraction(Player player)
	{
		
	}
}

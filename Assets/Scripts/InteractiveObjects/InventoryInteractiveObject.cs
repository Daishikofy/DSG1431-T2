using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteractiveObject : InterfaceInteractiveObject
{
    // Start is called before the first frame update
    void Start()
    {
        //var grid = FindObjectOfType<DynamicGrid()>;
		//grid.plceInGrid(transform.position);
		base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public override void onInteraction(Player player)
	{
		
	}
}

﻿using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollectibleItem : MonoBehaviour
{
    [SerializeField]
    string itemName = "None";

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entrou");
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.addToInventory(itemName);
            Destroy(this.gameObject);
        }
    }
}

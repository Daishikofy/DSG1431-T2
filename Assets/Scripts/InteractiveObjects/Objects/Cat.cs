using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : SolidInteractiveObject
{
    [SerializeField]
    CharacterManager manager;
    public override void onInteraction(Player player)
    {
        if (manager == null)
            manager = FindObjectOfType<CharacterManager>();
        manager.changeSkin("cat", player);
    }
}

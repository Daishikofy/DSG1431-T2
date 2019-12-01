using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private string currentLayer = "cat";
    public void changeSkin(string skinName, Player player)
    {
        var anim = player.GetComponent<Animator>();
        
        if (anim == null)
            Debug.Log("null");

        int currentLayerIndex = anim.GetLayerIndex(currentLayer);
        int skinIndex = anim.GetLayerIndex(skinName);
        Debug.Log("current: " + currentLayer + " - " + anim.GetLayerWeight(currentLayerIndex));
        Debug.Log("skin: " + skinName + " - " + anim.GetLayerWeight(skinIndex));
        anim.SetLayerWeight(skinIndex, 1f);
        anim.SetLayerWeight(currentLayerIndex, 0f);
        currentLayer = skinName;
 
        if (skinName == "cat")
        {
            player.GetComponent<Collider2D>().enabled = true;
            player.restrictController(false);
        }
        else
        {
            player.GetComponent<Collider2D>().enabled = false;
            player.restrictController(true);
        }
    }
}

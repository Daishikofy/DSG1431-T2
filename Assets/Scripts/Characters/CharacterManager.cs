using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private string currentLayer = "cat";
    public void changeSkin(string skinName, Player player)
    {
        if (currentLayer.CompareTo(skinName) == 0) return;
        var anim = player.GetComponent<Animator>();
        
        if (anim == null)
            Debug.Log("null");

        int currentLayerIndex = anim.GetLayerIndex(currentLayer);
        int skinIndex = anim.GetLayerIndex(skinName);
        //Debug.Log("current: " + currentLayer + " - " + anim.GetLayerWeight(currentLayerIndex));
        //Debug.Log("skin: " + skinName + " - " + anim.GetLayerWeight(skinIndex));
        anim.SetLayerWeight(skinIndex, 1f);
        anim.SetLayerWeight(currentLayerIndex, 0f);
        currentLayer = skinName;
 
        if (skinName == "cat")
        {  
            player.restrictController(false);
        }
        else
        {
            player.restrictController(true);
        }
    }
}

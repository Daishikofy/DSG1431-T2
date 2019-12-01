using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : SolidInteractiveObject
{
    SpriteRenderer renderer;
    [SerializeField]
    string keyName = "None";

    bool opening = false;
    public override void onInteraction(Player player)
    {
        if (opening) return;
        base.onInteraction(player);
        if (keyName == "None" || player.inventory.containsItem(keyName))
        {
            player.inventory.removeItem(keyName);
            StartCoroutine("fadeOut");
        }
    }

    protected IEnumerator fadeOut()
    {
        opening = true;
        float time = 1f;
        renderer = GetComponent<SpriteRenderer>();
        Color aux = new Color();
        aux = renderer.color;
        while (time > 0)
        {
            time -= Time.deltaTime;

            aux.a = time;
            renderer.color = aux;
            
            yield return null;
        }
        FindObjectOfType<DynamicGrid>().removeFromGrid(this.transform.position);
    }
}

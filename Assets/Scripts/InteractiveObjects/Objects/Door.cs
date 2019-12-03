using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : SolidInteractiveObject
{
    SpriteRenderer renderer;
    [SerializeField]
    string keyName = "None";
    [SerializeField]
    bool useObject;

    bool opening = false;
    public override void onInteraction(Player player)
    {
        if (opening) return;
        base.onInteraction(player);
        if (keyName == "None" || player.inventory.containsItem(keyName))
        {
            if (useObject)
                player.inventory.removeItem(keyName);
            StartCoroutine("fadeOut");
            FindObjectOfType<DynamicGrid>().removeFromGrid(this.transform.position);
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
        
    }

    public void open()
    {
        StartCoroutine("fadeOut");
        FindObjectOfType<DynamicGrid>().removeFromGrid(this.transform.position);
    }

    public void close()
    {
        FindObjectOfType<DynamicGrid>().placeInGrid(this.transform.position, this.gameObject);
    }
}

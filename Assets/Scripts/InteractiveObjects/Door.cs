using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : SolidInteractiveObject
{
    SpriteRenderer renderer;
    public override void onInteraction(Player player)
    {
        base.onInteraction(player);
        StartCoroutine("fadeOut");
    }

    protected IEnumerator fadeOut()
    {
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

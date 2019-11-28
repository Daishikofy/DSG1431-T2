using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : SolidInteractiveObject, IDamageable
{
    [SerializeField]
    private int maxLife = 1;
    [SerializeField]
    private int currentLife;
    [SerializeField]
    private Element element; //The element the object is sensible to. If none of the three use NONE

    protected override void Start ()
    {
        currentLife = maxLife;
        base.Start();
    }

    public void OnDamaged(int damage, Element element)
    {
        if (this.element == Element.None || this.element == element)
            currentLife -= damage;
        if (currentLife <= 0)
            StartCoroutine("fadeOut");
    }

    protected IEnumerator fadeOut()
    {
        float time = 1f;
        var renderer = GetComponent<SpriteRenderer>();
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

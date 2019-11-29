using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class spikes : MonoBehaviour
{
    [SerializeField]
    int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<DynamicGrid>().placeInGrid(transform.position, this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
            player.OnDamaged(damage, Element.None);
    }
}

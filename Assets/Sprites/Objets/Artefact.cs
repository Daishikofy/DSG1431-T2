using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Artefact : MonoBehaviour
{
    [SerializeField]
    string itemName = "None";
    [SerializeField]
    GameObject endGame;

    private void Start()
    {
        //TODO : good code please
        endGame = FindObjectOfType<BossFight>().endGame;
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entrou");
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            endGame.SetActive(true);
            player.addToInventory(itemName);
            Destroy(this.gameObject);
        }
    }
}

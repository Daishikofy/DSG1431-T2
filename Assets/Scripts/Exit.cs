using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private string levelName = "ThisLevel"; //TODO: ComboBox with scenes name?
    [SerializeField]
    private Vector2 position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO : Re implement part in the player's script in a goThroughDoor function
    //Add a fade in fade out
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Do not let ennemies and npc go througt the door.
        Debug.Log("Trigger with : " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            var movable = collision.gameObject.GetComponent<Movable>();
            if (!movable.isMoving)
                enterDoor(movable);
        }
    }

    private void enterDoor(Movable character)
    {
        //If the exit is in the same scene
        if (levelName == "ThisLevel")
        {
            FindObjectOfType<DynamicGrid>().moveInGrid(character.currentCell, position);
            character.currentCell = position;
            character.transform.position = position;
        }
        //TODO: Study if it would be more eficiente for the dynamicGrid to be static
        else
            Debug.Log("Movement between scenes yet to be implemented");
    }
}

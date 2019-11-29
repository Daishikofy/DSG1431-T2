﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private int levelIndex = -1; //TODO: ComboBox with scenes name?
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
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            player.stopForFrames(3);
            if (!player.isMoving)
                enterDoor(player);
        }
    }

    private void enterDoor(Player character)
    {
        //If the exit is in the same scene
        if (levelIndex == -1)
        {
            FindObjectOfType<DynamicGrid>().moveInGrid(character.currentCell, position);
            character.currentCell = position;
            character.transform.position = position;
        }
        //TODO: Study if it would be more eficiente for the dynamicGrid to be static
        else
        {
            FindObjectOfType<GameManager>().changeLevel(levelIndex);
            character.changeLevel(position);
        }
    }
}

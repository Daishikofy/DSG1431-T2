using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile_Name", menuName = "Custom/Attacks/Projectile Attack")]
public class ProjectileAttack : Attack
{
    [SerializeField]
    private Vector2[] startingZone;
    [SerializeField]
    private int range;

    public override void use(Fighter character)
    {
        base.use(character);

        DynamicGrid grid = FindObjectOfType<DynamicGrid>();
        Vector2 playerPosition = character.currentCell;
        Vector2[] affectedCells = new Vector2[startingZone.Length];
        Vector2[] directions = new Vector2[startingZone.Length];

        for (int i = 0; i < startingZone.Length; i++)
        {
            directions[i] = base.attackDirection(startingZone[i], character.getDirection());
            affectedCells[i] = directions[i] + playerPosition;
        }

        for (int i = 0; i < affectedCells.Length; i++)
        {
            //TODO : Quarternion rotation acording to direction[i]
            GameObject projectileObj = Instantiate(attack
                                                   , (Vector3)affectedCells[i]
                                                   , Quaternion.identity);
            projectileObj.GetComponent<Projectile>().Inicialize(directions[i], range, power, this);

            //DEBUG: Parte so para teste, quebra se usar ataque em outra coisa do que um movebleObject
            if (!grid.cellIsEmpty(affectedCells[i]))
            {/*
                GameObject target = grid.getInCell(affectedCells[i]);
                target.GetComponent<SpriteRenderer>().color = Color.black;*/
            }
        }
    }
}


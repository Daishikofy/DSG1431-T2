using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*The meelee move always happend with no restriction, it can actually be used as a distance move with no
 * need of field of view*/

[CreateAssetMenu(fileName = "Melee_Name", menuName = "Custom/Attacks/Melee Attack")]
public class MeleeAttack : Attack
{
    [SerializeField]
    private Vector2[] damageZone;

    public override void use(Fighter character)
    {
        base.use(character);

        DynamicGrid grid = FindObjectOfType<DynamicGrid>();
        Vector2 playerPosition = character.currentCell;
        Vector2[] affectedCells = new Vector2[damageZone.Length];

        //Determina quais são as casas afetadas pelo attaque baseado na posição inicial 
        //e na orientação do jogador
        for (int i = 0; i < damageZone.Length; i++)
        {
            Vector2 aux = base.attackDirection(damageZone[i], character.getDirection());
            affectedCells[i] = aux + playerPosition;
        }

        //Instancia a animação de impacto em todas as casas atingindas
        for (int i = 0; i < affectedCells.Length; i++)
        {
            Instantiate(attackObject, (Vector3)affectedCells[i], Quaternion.identity);

            //DEBUG: Parte so para teste, quebra se usar ataque em outra coisa do que um movebleObject   
            if (!grid.cellIsEmpty(affectedCells[i]))
            {
                GameObject target = grid.getInCell(affectedCells[i]);
                if (target != null)
                    giveDamage(target.GetComponent<Fighter>());               
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal_Name", menuName = "Custom/Attacks/Heal Moves")]
public class HealMove : Attack
{
    public override void use(Fighter character)
    {
        base.use(character);
        Instantiate(attackObject, (Vector3)character.currentCell, Quaternion.identity);
        character.addLife(power);     
    }
}

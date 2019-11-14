using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]
    protected string movesName;
    [SerializeField]
    protected int power;
    [SerializeField]
    protected int cost;
    [SerializeField]
    protected int element;
    [SerializeField]
    protected float cooldDown;
    /*[SerializeField]
    protected float stagger;
    */
    [SerializeField]
    protected GameObject attack;

    [SerializeField]
    protected Fighter character;

    public virtual void use(Fighter character)
    {
        this.character = character;
        ownerThrowBack(character);
    }

    protected Vector2 attackDirection(Vector2 relativePosition, Vector2 characterDirection)
    {
        Vector2 normal = new Vector2(characterDirection.y, characterDirection.x * -1);
        Vector2 res = relativePosition.x * normal + relativePosition.y * characterDirection;
        return res;
    }

    protected void ownerThrowBack(Fighter character)
    {
        character.addMana(cost * -1);
    }

    public void giveDamage(Fighter target)
    {
        target.OnDamaged(power);
        character.addCombo();
    }
}

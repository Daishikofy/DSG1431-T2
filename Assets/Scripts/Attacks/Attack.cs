using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Element { None, Fire, Water, Earth };

public abstract class Attack : ScriptableObject
{

    public Sprite icon;
    
    [SerializeField]
    protected string movesName;
    [SerializeField]
    protected int power;
    [SerializeField]
    protected float magicFactor;
    [SerializeField]
    protected float fisicFactor;
    [SerializeField]
    protected int cost;
    [SerializeField]
    protected Element element;
    [SerializeField]
    protected float cooldDown = 0.5f;

    [SerializeField]
    protected GameObject attackObject;

    protected Fighter character;
    bool isFinisher;
    public virtual void use(Fighter character)
    {
        this.character = character;
        ownerThrowBack();
    }

    public virtual void finisher(Fighter character)
    {
        this.character = character;
        isFinisher = true;
    }

    protected Vector2 attackDirection(Vector2 relativePosition, Vector2 characterDirection)
    {
        Vector2 normal = new Vector2(characterDirection.y, characterDirection.x * -1);
        Vector2 res = relativePosition.x * normal + relativePosition.y * characterDirection;
        return res;
    }

    protected void ownerThrowBack()
    {
        if (isFinisher)
        {
            character.AttackCoolDown(cooldDown + 1f);
        }
        else
            character.AttackCoolDown(cooldDown);
        character.addMana(cost * -1);
    }

    private int calculateDamage()
    {
        int damages = (int)(power + (character.magicPower * magicFactor) + (character.physicPower * fisicFactor));
        return damages;
    }
    public void giveDamage(IDamageable target)
    {
        int damages = calculateDamage();
        if (isFinisher)
        {
            damages = damages * 2;
            isFinisher = false;
        }
        else
            character.addCombo();
        target.OnDamaged(damages, element);
        
    }
}

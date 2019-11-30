using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Fighter : Movable, IDamageable
{
    [Space]
    //[SerializeField]
    public int maxLife; // TODO: Put Protected again, will brak in UI manager
    [SerializeField]
    protected int currentLife;
    protected bool attackCoolDown;

    [Space]
    //[SerializeField]
    public int maxMana; // TODO: Put Protected again, will brak in UI manager
    public int currentMana;
    [SerializeField]
    protected float manaCoolDown;
    //[SerializeField]
    protected bool manaIsRegenarating;

    [Space]
    //[SerializeField]
    public int magicPower;
    //[SerializeField]
    public int physicPower;

    [Space]
    [SerializeField]
    protected int combo;
    public int currentCombo;
    [SerializeField]
    protected float timeFromLastAttack;
    [SerializeField]
    protected float comboTiming;

    [Space]
    [SerializeField]
    private GameObject floatingText;
    [Space]
    [SerializeField]
    private float stagger = 0.25f;
    private bool stagged = false;
    [Space]
    [SerializeField]
    public Attack[] moveSet; // TODO: Botar privado novamente (vai quebrar no nivel do panel de attaques)
    [Space]

    [SerializeField]
    public IntEvent updateLifeUI;
    [SerializeField]
    public IntEvent updateManaUI;
    [SerializeField]
    public IntEvent updateComboUI;
    protected Vector2Int direction;
    protected Vector2Int movement;

    protected bool isKo = false;
    [HideInInspector]
    public UnityEvent Ko;

    // Start is called before the first frame update
    protected override void Start()
    {
        setMana(maxMana);
        setLife(maxLife);
        setCombo(0);
        direction = Vector2Int.down;
        base.Start();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentLife <= 0 && !isKo)
        {
            KnockOut();
        }
        else if (!isMoving && !onCooldown)
            if (stagged)
            {
                stagged = false;
                StartCoroutine(actionCooldown(stagger + coolDown));     
            }
    }

    protected IEnumerator ManaCoolDown(float coolDown)
    {
        manaIsRegenarating = true;
        float time = coolDown / maxMana;

        while (currentMana < maxMana)
        {
            addMana(1);
            yield return new WaitForSecondsRealtime(time);
            
            yield return null;
        }

        manaIsRegenarating = false;
    }
    public void AttackCoolDown(float coolDown)
    {
        StartCoroutine(AttackCoolDownCoroutine(coolDown));
    }
    protected IEnumerator AttackCoolDownCoroutine(float coolDown)
    {
        attackCoolDown = true;
        float time = coolDown;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        attackCoolDown = false;
    }

    public virtual void OnDamaged(int damage, Element element)
    {
        Debug.Log("On damage");
        if (floatingText)
            ShowFloatingText(damage);
        //TODO animator: Blink
        addLife(damage * -1);
        stagged = true;
    }

    protected virtual void KnockOut()
    {
        setLife(0);
        isKo = true;
        //For Debug purpose
        GetComponent<SpriteRenderer>().color = Color.black;
        //TODO - Animator: deathAnimation
        Debug.Log("Knock out: " + this.gameObject.name);
        grid.removeFromGrid(currentCell);
        Ko.Invoke();
        this.gameObject.SetActive(false);
    }

    public Vector2 getDirection()
    {
        return direction;
    }

    public void addMana(int mana)
    {
        setMana(currentMana + mana);
    }
    public void setMana(int mana)
    {
        currentMana = mana;

        if (currentMana> maxMana)
            currentMana = maxMana;
        else if (currentMana < 0)
            currentMana = 0;

        updateManaUI.Invoke(currentMana);
    }

    public void addLife(int life)
    {
        setLife(currentLife + life);
    }
    public void setLife(int life)
    {
        currentLife = life;

        if (currentLife > maxLife)
            currentLife = maxLife;
        else if (currentLife < 0)
            currentLife = 0;

        updateLifeUI.Invoke(currentLife);
    }
    public void addCombo()
    {
        setCombo(currentCombo+=1);
    }
    public void setCombo(int combo)
    {
        currentCombo = combo;
        updateComboUI.Invoke(currentCombo);
    }

    private void ShowFloatingText(int damage)
    {
        var obj = Instantiate(floatingText, this.transform.position, Quaternion.identity, this.transform);
        obj.GetComponent<TextMeshPro>().text = damage.ToString();
    }

}

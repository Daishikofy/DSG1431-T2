using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Torch : SolidInteractiveObject, IDamageable
{
    [SerializeField]
    private Element element; //The element the object is sensible to. If none of the three use NONE
    public bool isActivated = false;

    [HideInInspector]
    public UnityEvent stateChanged;
    Animator animator;
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        if (!isActivated)
        {
            if (animator == null)
                GetComponent<SpriteRenderer>().color = Color.blue;
            else
                animator.SetBool("Activated", false);
        }

        base.Start();
    }

    public void OnDamaged(int damage, Element element)
    {
        if (this.element == Element.None || this.element == element)
        {
            activate(true);
        }
    }

    public void setState(bool value)
    {
        activate(value);
    }

    private void activate(bool value)
    {
        if (value && !isActivated)
        {
            isActivated = true;
            stateChanged.Invoke();
        }
        else
            isActivated = value;

        if (animator)
            animator.SetBool("Activated", isActivated);
        else
            if (isActivated)
                GetComponent<SpriteRenderer>().color = Color.red;
            else
                GetComponent<SpriteRenderer>().color = Color.blue;
    }

}

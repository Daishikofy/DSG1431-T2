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
            GetComponent<SpriteRenderer>().color = Color.blue;
        base.Start();
    }

    public void OnDamaged(int damage, Element element)
    {
        if (this.element == Element.None || this.element == element)
        {
            if (!isActivated)
                isActivated = true;
            stateChanged.Invoke();
            if (animator)
                animator.SetBool("Activated", isActivated);
            else
                GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void setState(bool active)
    {
        isActivated = active;
        if (animator)
            animator.SetBool("Activated", isActivated);
        else
            GetComponent<SpriteRenderer>().color = Color.red;
    }

}

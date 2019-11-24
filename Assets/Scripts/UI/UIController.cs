using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI lifePoints;
    [SerializeField]
    TextMeshProUGUI manaPoints;
    [SerializeField]
    TextMeshProUGUI currentCombo;
    [SerializeField]
    Image powerPanel;
    bool powerPanelActivated = false;

    private CatsInput controller;

    // Start is called before the first frame update
    private void Start()
    {
        controller = new CatsInput();
        controller.Player.PrepAttack.Enable();
        controller.Player.PrepAttack.performed += context => showPowerPanel();
    }
    public void updateLifePoints(int life)
    {
        lifePoints.text = ("LIFE: " + life.ToString());
    }

    public void updateMana(int mana)
    {
        manaPoints.text = ("MANA: " + mana.ToString());
    }

    public void updateCombo(int combo)
    {
        if (combo == 0)
            currentCombo.text = "";
        else
            currentCombo.text = combo.ToString();
    }

    public void showPowerPanel()
    {
        Debug.Log("oi");
        if (powerPanelActivated)
            powerPanel.GetComponent<Animator>().SetBool("Selected", false);
        else 
            powerPanel.GetComponent<Animator>().SetBool("Selected", true);
        powerPanelActivated = !powerPanelActivated;
    }
}


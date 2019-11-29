using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Space]//Life
    [SerializeField]
    TextMeshProUGUI lifePoints;
    [SerializeField]
    Image lifeBar;
    int maxLife;

    [Space]//Mana
    [SerializeField]
    TextMeshProUGUI manaPoints;
    [SerializeField]
    Image manaBar;
    int maxMana;

    [Space]//Combo
    [SerializeField]
    TextMeshProUGUI currentCombo;
    [SerializeField]
    Image powerPanel;
    bool powerPanelActivated = false;

    [Space]//Powers panel
    [SerializeField]
    Sprite defaultIcon;
    [SerializeField]
    Image[] powerImages;
    private CatsInput controller;

    [Space]//Pause menu
    [SerializeField]
    GameObject pausedMenu;

    // Start is called before the first frame update
    private void Start()
    {
        controller = new CatsInput();
        controller.Player.PrepAttack.Enable();
        controller.Player.PrepAttack.performed += context => showPowerPanel();
        setIcons();
        Player player = FindObjectOfType<Player>();
        maxLife = player.maxLife;
        maxMana = player.maxMana;

        pausedMenu.SetActive(false);
    }

    private void setIcons()
    {
        //TODO: Dear futur self. This code is really bad, needs improvement but I am to tired to make it good right know 
        Attack[] attacks = FindObjectOfType<Player>().moveSet;
        var attackIcons = new Sprite[4];
        int i;
        for (i = 0; i < attacks.Length; i++)
        {
            attackIcons[i] = attacks[i].icon;
        }
        for (; i < attackIcons.Length; i++)
        {
            attackIcons[i] = defaultIcon;
        }
        for (i = 0; i < attackIcons.Length; i++)
        {
            powerImages[i].sprite = attackIcons[i];
        }
    }
    public void updateLifePoints(int life)
    {
        lifePoints.text = life.ToString();
        lifeBar.fillAmount = (float)life / (float)maxLife;
    }

    public void updateMana(int mana)
    {
        manaPoints.text = mana.ToString();
        manaBar.fillAmount = (float)mana / (float)maxMana;
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


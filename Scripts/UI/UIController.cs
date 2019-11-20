using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI lifePoints;
    [SerializeField]
    TextMeshProUGUI manaPoints;
    [SerializeField]
    TextMeshProUGUI currentCombo;
    
    // Start is called before the first frame update

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
}


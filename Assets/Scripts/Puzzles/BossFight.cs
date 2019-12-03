using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFight : Puzzles
{
    bool playerEntered = false;

    [SerializeField]
    Door door;
    [SerializeField]
    BossCat boss;

    [SerializeField]
    Image bossUI;
    [SerializeField]
    Image lifeBar;
    // Start is called before the first frame update
    void Start()
    {
        boss.Ko.AddListener(bossIsKo);
        boss.updateLifeUI.AddListener(updateLifeBar);
        boss.enabled = false;
        bossUI.gameObject.SetActive(false);
        door.open();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (playerEntered) return;
        //Audio - music: End dungeon, start boss battle
        StartCoroutine(waitToCloseDoor());
    }

    private void bossIsKo()
    {

        //Audio - music: End boss battle, start dungeon
        door.open();
        bossUI.gameObject.SetActive(false);
        
        completed();
    }

    private void updateLifeBar(int life)
    {
        lifeBar.fillAmount = (float)life / (float)boss.maxLife;
    }

    private IEnumerator waitToCloseDoor()
    {
        yield return new WaitForSeconds(1f);
        door.close();
        playerEntered = true;
        boss.enabled = true;
        bossUI.gameObject.SetActive(true);
    }

}

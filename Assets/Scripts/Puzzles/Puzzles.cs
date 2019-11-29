using UnityEngine;

public class Puzzles : MonoBehaviour
{
    [SerializeField]
    GameObject reward;
    [SerializeField]
    Vector2 rewardSpawnPoint;
    bool puzzleSolved = false;
    protected void completed()
    {
        if (puzzleSolved == true) return;
        //SOM : Recompensa
        Vector3 aux = new Vector3(rewardSpawnPoint.x, rewardSpawnPoint.y, 0);
        Instantiate(reward, aux, Quaternion.identity);
        puzzleSolved = true;
    }
}

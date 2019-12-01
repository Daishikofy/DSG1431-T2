using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBehaviours : MonoBehaviour
{
    private CatsInput controller;
    // Start is called before the first frame update
    void Awake()
    {
        controller = new CatsInput();
        controller.Enable();
        controller.Player.Restart.performed += context => Restart();
    }

    // Update is called once per frame
    void Restart()
    {
        GetComponent<GameManager>().resetGame();       
    }
}

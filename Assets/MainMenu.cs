using UnityEngine;
using UnityEngine.sceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene("TestScene");
    }

}

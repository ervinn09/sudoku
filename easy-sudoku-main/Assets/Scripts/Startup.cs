using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    /// <summary>
    /// Go to the GameScene, 
    /// whenever Play or Play Again is clicked
    /// </summary>
    public void ButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Quit the app,
    /// when exit is clicked
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}

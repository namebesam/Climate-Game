using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this line to access SceneManager


 
public class MainMenu : MonoBehaviour
{
    // This function will be called when the button is clicked
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // Loads the scene with the given name
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    
    public void Tutorial()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

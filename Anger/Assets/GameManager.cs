using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    bool gameHasEnded = false;
    public Score GameScore;

    public void EndGame()
    {
        if (!gameHasEnded)
        {
            //delay = Time.time;
            gameHasEnded = true;
            Debug.Log("Koniec kiereczki");
            Restart();
        }
    }

    void Restart()
    {
        //FindObjectOfType<Score>().delay = Time.time;
        SceneManager.LoadScene("MainMenu");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

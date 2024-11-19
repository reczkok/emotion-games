using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class resetTutorial : MonoBehaviour
{
    public
    //public GameManager GM;
    void OnCollisionEnter(Collision collisionInfo)
    {

        //Debug.Log(collisionInfo.collider.name);
        if (collisionInfo.GetComponent<Collider>().name == "base")
        {
            SceneManager.LoadScene(5);

            //FindObjectOfType<SCOREBOARD>().UpdateScoreboard();
        }
    }
    void Update()
    {
        // Check if the space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Change the scene to scene number 3
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}

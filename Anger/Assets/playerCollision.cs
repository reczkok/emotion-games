using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollision : MonoBehaviour
{
    public
    //public GameManager GM;
    void OnCollisionEnter(Collision collisionInfo)
    {
        
        //Debug.Log(collisionInfo.collider.name);
        if (collisionInfo.GetComponent<Collider>().name == "base")
        {
            Debug.Log(FindObjectOfType<Score>().score);
            //FindObjectOfType<SCOREBOARD>().UpdateScoreboard();
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}

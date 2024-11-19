using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Score : MonoBehaviour
{

    float timer;
    float delay;
    public int score;
    public Text  scoreText;
    // Update is called once per frame
    void Update()
    {
        timer = Time.timeSinceLevelLoad;
        if (timer < 50)
        {
            score = (int)timer;
        }
        else if(timer < 100)
        {
            score = (int)(timer * 0.8 + 10);
        }
        else
        {
            score = (int)(timer * 0.5 + 40);
        }
        //score = (int)((timer + Math.Pow(1.1, (timer - 50))) % 100f);
        scoreText.text = score.ToString();
        
        //scoreText.text = "siemaneczko";
        //Debug.Log(Time.time.ToString());
        if (score >= 100)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class SCOREBOARD : MonoBehaviour
{
    public Text scoreText;
    public List<int> scores = new List<int> {0, 0, 0, 0, 0, 0};


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(scores);
        string scoreboard = GenerateScoreboard(scores);
        Debug.Log(scoreboard);
        scoreText.text = scoreboard;
    }

    string GenerateScoreboard(List<int> scores)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < scores.Count; i++)
        {
            sb.AppendLine($"{i + 1}. Score: {scores[i]}");
        }

        return sb.ToString();
    }
    public  void UpdateScoreboard(int score)
    {
        Debug.Log(GenerateScoreboard(scores));
        if (score < scores[scores.Count - 1])
        {
            scores[scores.Count - 1] = score;

            scores.Sort();
            scores.Reverse();
        }
    }
}

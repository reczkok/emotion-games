using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{

    int breakableBlocks;
    [Range(1, 1.5f)] [SerializeField] float LevelSpeed=1f;
    // Start is called before the first frame update
    void Start()
    {
        CountBreakableBlocks();
        LevelSpeed = 1f;
        Time.timeScale = LevelSpeed;
    }

    public void CountBreakableBlocks()
    {
        var allLevelBlocks = FindObjectsByType<Block>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var block in allLevelBlocks)
        {
            if (block.CompareTag("Breakable"))
            {
                breakableBlocks++;
            }
        }

    }

    public void RemoveBlock()
    {
        breakableBlocks--;
        if(breakableBlocks == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (breakableBlocks % 5 != 0) return;
        
        LevelSpeed += 0.1f;
        LevelSpeed = Mathf.Clamp(LevelSpeed, 1, 1.5f);
        Time.timeScale = LevelSpeed;
    }
}

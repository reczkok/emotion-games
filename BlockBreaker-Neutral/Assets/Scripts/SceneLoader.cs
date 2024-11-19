using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Levels = new();
    int levelIndex = -1;
    GameObject currentlySpawned;
    Level Level;
    
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 2) return;
        
        DontDestroyOnLoad(gameObject);
        Level = FindFirstObjectByType<Level>();
        LoadNextScene();
    }
    
    public void LoadNextScene()
    {
        levelIndex += 1;
        if(levelIndex >= 2)
        {
            SceneManager.LoadScene(2);
        }
        if(currentlySpawned != null)
        {
            Destroy(currentlySpawned);
        }
        Debug.Log(Levels[levelIndex]);
        currentlySpawned = Instantiate(Levels[levelIndex], transform);
    }

    public void LoadPreviousScene()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex != 0)
            SceneManager.LoadScene(0);
    }

    public void LoadByName(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ReloadCurrentLevel()
    {
        if (currentlySpawned != null)
        {
            //    Destroy(currentlySpawned);
        }
        FindFirstObjectByType<Ball>()?.Reset();
        FindFirstObjectByType<Paddle>()?.Reset();
      //  currentlySpawned = Instantiate(Levels[levelIndex], Level.gameObject.transform);
    }

}

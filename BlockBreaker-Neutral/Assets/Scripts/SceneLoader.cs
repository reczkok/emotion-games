using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private List<GameObject> levels = new();
    private int levelIndex = -1;
    private GameObject currentlySpawned;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 2) return;
        
        DontDestroyOnLoad(gameObject);
        FindFirstObjectByType<Level>();
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
        currentlySpawned = Instantiate(levels[levelIndex], transform);
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
      //  currentlySpawned = Instantiate(Levels[levelIndex], Level.gameObject.transform);
    }

}

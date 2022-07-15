using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Levels = new List<GameObject>();
    int levelIndex = -1;
    GameObject currentlySpawned = null;
    Level Level;
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
        currentlySpawned = Instantiate(Levels[levelIndex], Level.gameObject.transform);
       
    }

    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex != 0)
        SceneManager.LoadScene(0);
    }

    public void LoadByName(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex < 2)
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            Level = FindObjectOfType<Level>();
            LoadNextScene();
        }
    }

    public void ReloadCurrentLevel()
    {
        if (currentlySpawned != null)
        {
        //    Destroy(currentlySpawned);
        }
        FindObjectOfType<Ball>()?.Reset();
        FindObjectOfType<Paddle>()?.Reset();
      //  currentlySpawned = Instantiate(Levels[levelIndex], Level.gameObject.transform);
    }

}

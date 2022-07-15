using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    // Start is called before the first frame update
    Button button;

    void Start()
    {
        if (gameObject != null)
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => QuitApp());
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

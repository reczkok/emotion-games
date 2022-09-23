using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnalyticsInputController : MonoBehaviour
{
    TMP_InputField playerInput;
    Button acceptButton;
    Ball ball;
    Paddle paddle;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInChildren<TMP_InputField>();
        acceptButton = GetComponentInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
      if(playerInput.text != null && playerInput.text.Length > 0)
      {
            acceptButton.interactable = true;
      } else {
            acceptButton.interactable = false;
      }
    }

    public void Hide()
    {
       paddle.canMove = true;
       ball.canLaunch = true;
       gameObject.SetActive(false);
    }

    public async void SetAnalyticsName()
    {
        await AnalyticsController.Instance.SetupAnalytics(playerInput.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}

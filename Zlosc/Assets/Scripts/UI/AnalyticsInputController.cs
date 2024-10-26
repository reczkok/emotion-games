using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnalyticsInputController : MonoBehaviour
{
    TMP_InputField playerInput;
    Button acceptButton;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        playerInput = GetComponentInChildren<TMP_InputField>();
        acceptButton = GetComponentInChildren<Button>();
        playerController.controlEnabled = false;
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
       playerController.controlEnabled = true;
       playerController.jumpState = PlayerController.JumpState.Grounded;
       gameObject.SetActive(false);
    }

}

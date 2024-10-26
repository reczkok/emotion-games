using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsTextController : MonoBehaviour
{
    TextMeshProUGUI controlsText;
    PlayerController playerController;
    // Start is called before the first frame update
    void Awake()
    {
        controlsText = GetComponent<TextMeshProUGUI>();
    }


    private void OnEnable()
    {
        SetControls();
    }

    public void SetControls()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        controlsText.text = $"Lewo: {playerController.leftKey} \n\nPrawo: {playerController.rightKey}\n\nSkok: {playerController.jumpKey}";
    }
}

using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoesInteraction : MonoBehaviour, Interactable
{
    public bool isInteractable = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }

    public void OnAction()
    {

    }
}
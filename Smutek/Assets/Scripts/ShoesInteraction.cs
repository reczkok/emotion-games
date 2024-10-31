using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoesInteraction : MonoBehaviour, Interactable
{
    public bool isInteractable = true;

    public bool IsInteractable()
    {
        return isInteractable;
    }

    public void OnAction()
    {

    }
}

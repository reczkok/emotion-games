using Assets.Scripts;
using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoInteraction : MonoBehaviour
{
    public bool isInteractable = true;
    private bool _isHeld;
    private Vector3 _photoPos;
    private Quaternion _photoRot;
    private Camera _mainCamera;
    private Transform _cameraTransform;

    public bool IsInteractable()
    {
        return isInteractable;
    }

    public void PickedUp()
    {
        if (_isHeld || !isInteractable) return;
        Debug.Log("Zoom In");
        _isHeld = true;
        var dialog = GetComponent<DialogueTrigger>();
        if (dialog)
        {
            dialog.TriggerDialogue();
        }
    }
    
    public void PutDown()
    {
        if (!_isHeld) return;
        _isHeld = false;
        isInteractable = false;
        EventManager.TriggerEvent(UnityEvents.PHOTO_DOWN);
    }
}

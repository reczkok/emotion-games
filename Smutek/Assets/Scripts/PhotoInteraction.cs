using System;
using Assets.Scripts;
using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoInteraction : MonoBehaviour
{
    public bool isInteractable = true;
    private bool _isHeld;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = FindFirstObjectByType<Canvas>();
        Debug.Log(_canvas);
    }

    public void PickedUp()
    {
        if (_isHeld || !isInteractable) return;
        Debug.Log("Zoom In");
        StartCoroutine(SetIsHeld());
        var dialog = GetComponent<DialogueTrigger>();
        if (!dialog) return;

        RepositionCanvas();
        dialog.TriggerDialogue();
    }
    
    private IEnumerator SetIsHeld()
    {
        yield return new WaitForSeconds(1);
        _isHeld = true;
    }
    
    private void RepositionCanvas()
    {
        _canvas.transform.SetParent(transform);
        _canvas.transform.localScale = new Vector3(0.2f, 0.3f, 0.3f);
        _canvas.transform.localPosition = new Vector3(0, 5, 1);
        _canvas.transform.localRotation = Quaternion.Euler(90, 0, 0);
        _canvas.enabled = true;
    }
    
    public void PutDown()
    {
        if (!_isHeld) return;
        _isHeld = false;
        isInteractable = false;
        EventManager.TriggerEvent(UnityEvents.PhotoDown);
    }
}

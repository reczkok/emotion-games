using Assets.Scripts;
using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoInteraction : MonoBehaviour, Interactable
{
    public bool isInteractable = true;
    public float zoomTime = 3.0f;
    public float rotTime = 3.0f;
    private bool isZommed = false;
    private Vector3 photoPos;
    private Quaternion photoRot;
    public bool IsInteractable()
    {
        return isInteractable;
    }

    public void OnAction()
    {
        if (!isZommed)
        {
            Debug.Log("Zoom In");
            EventManager.TriggerEvent(UnityEvents.CAMERA_LOCK);
            var camera = Camera.main.transform;
            photoPos = transform.position;
            photoRot = transform.rotation;
            StartCoroutine(LerpPhotoToPos(photoPos, camera.position + camera.forward * 0.3f ));
            StartCoroutine(LerpToCameraRotation(photoRot, Quaternion.LookRotation(camera.up)));
            isZommed = true;
            var dialog = GetComponent<DialogueTrigger>();
            if (dialog)
            {
                dialog.TriggerDialogue();
            }
        } else
        {
            isZommed = false;
            isInteractable = false;
            StartCoroutine(LerpPhotoToPos(transform.position, photoPos));
            StartCoroutine(LerpToCameraRotation(transform.rotation, photoRot));
            EventManager.TriggerEvent(UnityEvents.CAMERA_UNLOCK);
            EventManager.TriggerEvent(UnityEvents.PHOTO_DOWN);
        }

    }

    public IEnumerator LerpPhotoToPos(Vector3 startPos, Vector3 endPos)
    {
        float timeElapsed = 0;

        while (timeElapsed < rotTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / zoomTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }

    public IEnumerator LerpToCameraRotation(Quaternion startRot, Quaternion endRot)
    {
        float timeElapsed = 0;

        while (timeElapsed < zoomTime)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, timeElapsed / rotTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRot;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

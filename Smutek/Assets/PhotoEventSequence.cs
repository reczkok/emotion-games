using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PhotoEventSequence : MonoBehaviour
{
    [FormerlySerializedAs("AfterPhotosDialogue")] public Dialogue afterPhotosDialogue;
    private const int PhotoNumber = 5;
    private int _photosUsed;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(UnityEvents.PhotoDown, PhotoUsed);
    }

    private void PhotoUsed()
    {
        _photosUsed += 1;
        Debug.Log("Photo Used");
        if (_photosUsed < PhotoNumber) return;
        CanvasPositioner.Instance.RepositionCanvas(GameObject.FindWithTag("PosPhotos").transform);
        DialogueManager.Instance.StartDialogue(afterPhotosDialogue);
        EventManager.TriggerEvent(UnityEvents.EndPhotoEvent);
        EventManager.TriggerEvent(UnityEvents.MovementUnlock);
    }

}

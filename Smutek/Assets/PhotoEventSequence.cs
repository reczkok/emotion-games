using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PhotoEventSequence : MonoBehaviour
{
    [FormerlySerializedAs("AfterPhotosDialogue")] public Dialogue afterPhotosDialogue;
    private int _photoNumber = 6;
    private int _photosUsed;
    // Start is called before the first frame update
    void Start()
    {
        _photoNumber = GetComponentsInChildren<PhotoInteraction>().Length;
        EventManager.StartListening(UnityEvents.PHOTO_DOWN, PhotoUsed);
    }

    public void PhotoUsed()
    {
        _photosUsed += 1;
        Debug.Log("Photo Used");
        if (_photosUsed < _photoNumber) return;
        DialogueManager.Instance.StartDialogue(afterPhotosDialogue);
        EventManager.TriggerEvent(UnityEvents.END_PHOTO_EVENT);
    }

}

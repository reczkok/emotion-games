using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotoEventSequence : MonoBehaviour
{
    public Dialogue AfterPhotosDialogue;
    private int photoNumber = 6;
    private int photosUsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        photoNumber = GetComponentsInChildren<PhotoInteraction>().Length;
        EventManager.StartListening(UnityEvents.PHOTO_DOWN, PhotoUsed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PhotoUsed()
    {
        photosUsed += 1;
        if(photosUsed >= photoNumber)
        {
            DialogueManager.Instance.StartDialogue(AfterPhotosDialogue);
            EventManager.TriggerEvent(UnityEvents.END_PHOTO_EVENT);
        }
    }

}

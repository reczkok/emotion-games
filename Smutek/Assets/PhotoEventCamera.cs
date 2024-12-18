using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoEventCamera : MonoBehaviour
{
    public Vector3 endPos = new Vector3(0, 0.903999984f, 0.0540000014f);
    public float getUpTime = 2.0f;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(UnityEvents.END_PHOTO_EVENT, MoveCameraUp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCameraUp()
    {
        startPos = transform.localPosition;
        StartCoroutine(LerpCameraToPos(endPos));
    }

    IEnumerator LerpCameraToPos(Vector3 pos)
    {
        float timeElapsed = 0;

        while (timeElapsed < getUpTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, timeElapsed / getUpTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPos;
        EventManager.TriggerEvent(UnityEvents.MOVEMENT_UNLOCK);
    }

}

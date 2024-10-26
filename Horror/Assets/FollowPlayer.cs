using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject Wheelchair;
    public AudioClip WheelchairAudio;
    private bool isFollowing = false;
    public float positionTime = 0.5f;
    private Vector3 positionToGo;
    private Quaternion startingRotation;
    public float speed = 3.0f;
    private Transform playerTransform;
    private AudioSource WheelChairAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        WheelChairAudioSource = Wheelchair.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFollowing || !Wheelchair || !playerTransform) return;
        if (Vector3.Distance(Wheelchair.transform.position, playerTransform.position) < 3f)
        {
            return;
        }
        Wheelchair.transform.position = Vector3.MoveTowards(Wheelchair.transform.position, playerTransform.position, speed * Time.deltaTime);
        Wheelchair.transform.rotation = Quaternion.RotateTowards(Wheelchair.transform.rotation, startingRotation, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isFollowing = true;
            playerTransform = other.gameObject.transform;
            WheelChairAudioSource.clip = WheelchairAudio;
            WheelChairAudioSource.Play();
            StartCoroutine(SetupPosition());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isFollowing = false;
            WheelChairAudioSource.Stop();
            StopAllCoroutines();
        }
    }

    private IEnumerator SetupPosition()
    {
        while (isFollowing)
        {
            positionToGo = playerTransform.position;
            startingRotation = Wheelchair.transform.rotation;
            yield return new WaitForSeconds(positionTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public List<AudioSource> audiosToPlay = new List<AudioSource>();
    public int maxWaitTime = 5;
    public int minWaitTime = 1;
    private bool isAudioPlaying = false;
    int playSound = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isAudioPlaying && playSound < 2)
        {
            isAudioPlaying = true;
            StartCoroutine(WaitAndPlaySound(Random.Range(minWaitTime, maxWaitTime)));
        }
    }

    IEnumerator WaitAndPlaySound(int waitForSec)
    {
        yield return new WaitForSeconds(waitForSec);

        audiosToPlay[playSound].Play();
        isAudioPlaying = false;
        playSound++;
    }




}

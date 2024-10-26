using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Flashlight : MonoBehaviour
{
    [FormerlySerializedAs("MaxFlicker")] public int maxFlicker = 10;
    [FormerlySerializedAs("MinFlicker")] public int minFlicker = 2;
    [FormerlySerializedAs("TimeDelay")] public float timeDelay = 0.2f;
    private Light lightSource;
    public bool isFlickering;
    private StarterAssetsInputs input;
    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light>();
        input = FindFirstObjectByType<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!input.flashlight) return;
        if (isFlickering) return;
        isFlickering = true;
        input.flashlight = false;
        StartCoroutine(FlashlightFlicker(Random.Range(minFlicker,maxFlicker)));
    }

    public IEnumerator FlashlightFlicker(int flickerAmount)
    {
        for(var i=0; i<flickerAmount; i++)
        {
            lightSource.enabled = false;
            yield return new WaitForSeconds(timeDelay + Random.Range(0, 0.3f));
            lightSource.enabled = true;
            yield return new WaitForSeconds(timeDelay);
        }
        isFlickering = false;
    }
}

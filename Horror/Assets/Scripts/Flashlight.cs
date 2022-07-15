using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public int MaxFlicker = 10;
    public int MinFlicker = 2;
    public float TimeDelay = 0.2f;
    private Light Light;
    public bool isFlickering = false;
    private StarterAssetsInputs Input;
    // Start is called before the first frame update
    void Start()
    {
        Light = GetComponent<Light>();
        Input = FindObjectOfType<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.flashlight)
        {
            if (!isFlickering)
            {
                isFlickering = true;
                Input.flashlight = false;
                StartCoroutine(FlashligthFlicker(Random.Range(MinFlicker,MaxFlicker)));
            }
        }
    }

    public IEnumerator FlashligthFlicker(int flickerAmount)
    {
        for(int i=0; i<flickerAmount; i++)
        {
            Light.enabled = false;
            yield return new WaitForSeconds(TimeDelay + Random.Range(0, 0.3f));
            Light.enabled = true;
            yield return new WaitForSeconds(TimeDelay);
        }
        isFlickering = false;
    }
}

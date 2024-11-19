using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ResultCalculator : MonoBehaviour
{
    public static ResultCalculator Instance { get; private set; }
    public FinalObjectComponent finalObjectToUnlock;
    public float finalValue = 7.0f;
    public float currentValue;

    // Start is called before the first frame update
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddValue(float valueToAdd)
    {
        currentValue += valueToAdd;
        Debug.Log("Current Value: " + currentValue);
        if(currentValue >= finalValue)
        {
            finalObjectToUnlock.ItemsCollected();
        }
    }
}

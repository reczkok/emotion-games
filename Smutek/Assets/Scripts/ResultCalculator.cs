using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ResultCalculator : MonoBehaviour
{
    public static ResultCalculator Instance { get; private set; }
    [FormerlySerializedAs("FinalObjectToUnlock")] public FinalObjectComponent finalObjectToUnlock;
    [FormerlySerializedAs("FinalValue")] public float finalValue = 8.0f;
    [FormerlySerializedAs("CurrentValue")] public float currentValue;

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
        if(currentValue >= finalValue)
        {
            finalObjectToUnlock.ItemsCollected();
        }
    }
}

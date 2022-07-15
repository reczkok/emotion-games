using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCalculator : MonoBehaviour
{
    public static ResultCalculator Instance { get; private set; }
    public FinalObjectComponent FinalObjectToUnlock;
    public float FinalValue = 8.0f;
    public float CurrentValue = 0.0f;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddValue(float valueToAdd)
    {
        CurrentValue += valueToAdd;
        if(CurrentValue >= FinalValue)
        {
            FinalObjectToUnlock.ItemsCollected();
        }
    }
}

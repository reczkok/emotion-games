using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR;

public class Controller_Test : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody target;
    [SerializeField] private float factor;
    [SerializeField] private float movementLimit;

    private InputData _inputData;

    void Start()
    {
        _inputData = GetComponent<InputData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightVelocity))
        {
            float x = rightVelocity[0] * factor;
            float y = rightVelocity[2] * factor;
            float z = rightVelocity[1] * factor;
            x = (x > 0) ? Math.Min(x, movementLimit) : Math.Max(x, -movementLimit);
            y = (y > 0) ? Math.Min(y, movementLimit) : Math.Max(y, -movementLimit);
            z = (z > 0) ? Math.Min(z, movementLimit) : Math.Max(z, -movementLimit);
            Vector3 transformVector = new Vector3(x, z, y);

            target.Move(target.position + transformVector, target.rotation);
        }
    }
}

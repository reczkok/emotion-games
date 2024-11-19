using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Mouse_Test : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody target;
    [SerializeField] private float factor;
    [SerializeField] private float movementLimit;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X") * factor;
        float y = Input.GetAxis("Mouse Y") * factor;
        x = (x > 0) ? Math.Min(x, movementLimit) : Math.Max(x, -movementLimit);
        y = (y > 0) ? Math.Min(y, movementLimit) : Math.Max(y, -movementLimit);
        Vector3 transformVector = new Vector3(x, 0, y);

        target.Move(target.position + transformVector, target.rotation);
    }
}

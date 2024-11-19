//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class fingermovement : MonoBehaviour


{
    public Rigidbody rb;
    int force = 100;
    public Vector3 newPosition;
    bool isCorrected = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float mouseX = Input.GetAxis("Mouse X");
        float mouseZ = Input.GetAxis("Mouse Y");
        Debug.Log("Mouse X: "+ mouseX);
        Debug.Log("Mouse Z: "+ mouseZ);
        if (!(isCorrected) && ((mouseX != 0) || (mouseZ != 0)))
        {
            Debug.Log("Correcting in update");
            Debug.Log("transform.position befor correction: " + transform.position);
            transform.position = new Vector3(-mouseX * force * Time.deltaTime, 0, -mouseZ * force * Time.deltaTime);
            Debug.Log("transform.position after correction: " + transform.position);
            isCorrected = true;
        }
        transform.position += new Vector3(mouseX * force *Time.deltaTime, 0, mouseZ * force * Time.deltaTime);
        Debug.Log("transform.position: " + transform.position);
        //rb.AddForce(mouseX * force * Time.deltaTime, 0, 0);
        //rb.AddForce(0, 0, mouseZ * force);

        //rb = GetComponent<Rigidbody>();
        //rb.AddForce(Vector3.forward, Forcemode.Impulse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Following : MonoBehaviour
{

    // Start is called before the first frame update
    public float speed;
    public Transform target;
    void Start()
    {
        GameObject targetObject = GameObject.Find("pole");
        target = targetObject.transform;
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
        transform.up = target.transform.position - transform.position;
    }
}

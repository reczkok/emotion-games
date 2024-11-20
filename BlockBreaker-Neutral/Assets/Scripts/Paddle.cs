using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Paddle : MonoBehaviour
{

    [SerializeField] float ScreenWidthInUnits = 16f;
    [SerializeField] float minX = 1f;
    [SerializeField] float maxX = 15f;
    Vector2 startPos;
    public bool canMove;
    public GameObject xrGrabObj;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        xrGrabObj = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        
        var grabObjX = xrGrabObj.transform.position.x;
        var newX = Mathf.Clamp(grabObjX, minX, maxX);
        transform.position = new Vector2(newX, transform.position.y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    [SerializeField] float ScreenWidthInUnits = 16f;
    [SerializeField] float minX = 1f;
    [SerializeField] float maxX = 15f;
    Vector2 startPos;
    public bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        
        var mouseXinUnits = Input.mousePosition.x / Screen.width * ScreenWidthInUnits;
        mouseXinUnits = Mathf.Clamp(mouseXinUnits, minX, maxX);
        var newPosition = new Vector2(mouseXinUnits, transform.position.y);
        transform.position = newPosition;
    }

    public void Reset()
    {
        transform.position = startPos;
    }
}

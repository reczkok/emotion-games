using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool isMovable = true;
    public float speedUpRandomizer = 0.2f;
    public float speedUpValue = 1.0f;
    public float maxYOffset = 0.0f;
    public float maxXoffset = 1.0f;
    public float duration = 5;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool reverse = false;
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position - new Vector3(maxXoffset, maxYOffset, 0);
        endPos = transform.position + new Vector3(maxXoffset, maxYOffset, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovable && !isMoving)
        {
            isMoving = true;
            StartCoroutine(LerpPosition(reverse ? startPos : endPos, duration));
        }
    }

    IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        if(Random.value < speedUpRandomizer)
        {
            duration -= speedUpValue;
        }
        Vector2 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        reverse = !reverse;
        isMoving = false;
    }
}

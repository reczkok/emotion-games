using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Paddle paddle= null;
    [SerializeField] float LaunchHeight = 15f;
    [SerializeField] float LaunchSlide = 2f;
    [SerializeField] float RandomFactor = 0.2f;
    [SerializeField] AudioClip[] hitSounds = null;

    Vector2 paddleToBallVector;
    Rigidbody2D rigidbody;
    bool isLaunched = false;
    public bool canLaunch = false;
    Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        if (paddle != null)
        {
            paddleToBallVector = transform.position - paddle.transform.position;
        }
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLaunched && canLaunch)
        {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isLaunched = true;
            rigidbody.velocity = new Vector2(LaunchSlide,LaunchHeight);
        }
    }

    private void LockBallToPaddle()
    {
        if (paddle != null)
        {
            Vector2 paddlePos = new Vector2(paddle.transform.position.x, paddle.transform.position.y);
            transform.position = paddlePos + paddleToBallVector;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2(
            UnityEngine.Random.Range(0, RandomFactor), 
            -UnityEngine.Random.Range(0, RandomFactor)
            );

        if (isLaunched)
        {
            AudioClip clip = hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)];
            GetComponent<AudioSource>().PlayOneShot(clip);
            rigidbody.velocity += velocityTweak;
        }
    }

    public void Reset()
    {
        rigidbody.velocity = Vector2.zero;
        transform.position = startPos;
        canLaunch = true;
        isLaunched = false;
        
    }
}

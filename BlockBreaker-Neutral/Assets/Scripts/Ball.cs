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
    Rigidbody2D ballBody;
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
        ballBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLaunched || !canLaunch) return;
        
        LockBallToPaddle();
        LaunchOnControllerA();
    }

    private void LaunchOnControllerA()
    {
        // Launch on any XR controller button
        if (!Input.GetKeyDown(KeyCode.Joystick1Button0)) return;
        
        isLaunched = true;
        ballBody.linearVelocity = new Vector2(LaunchSlide,LaunchHeight);
    }

    private void LockBallToPaddle()
    {
        if (!paddle) return;
        
        var paddlePos = new Vector2(paddle.transform.position.x, paddle.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var velocityTweak = new Vector2(
            UnityEngine.Random.Range(0, RandomFactor), 
            -UnityEngine.Random.Range(0, RandomFactor)
            );

        if (!isLaunched) return;
        
        var clip = hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)];
        GetComponent<AudioSource>().PlayOneShot(clip);
        ballBody.linearVelocity += velocityTweak;
    }

    public void Reset()
    {
        ballBody.linearVelocity = Vector2.zero;
        LockBallToPaddle();
        canLaunch = true;
        isLaunched = false;
    }
}

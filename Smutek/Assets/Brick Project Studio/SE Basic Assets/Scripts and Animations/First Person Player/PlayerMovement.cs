using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
    public class PlayerMovement : MonoBehaviour
    {

        public CharacterController controller;

        public float speed = 5f;
        public float gravity = -15f;
        public bool lockMovement = false;

        Vector3 velocity;

        bool isGrounded;
        private void Start()
        {
            EventManager.StartListening(UnityEvents.MOVEMENT_LOCK, LockMovement);
            EventManager.StartListening(UnityEvents.MOVEMENT_UNLOCK, UnlockMovement);
        }
        // Update is called once per frame
        void Update()
        {
            if (!lockMovement)
            {
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                Vector3 move = transform.right * x + transform.forward * z;

                controller.Move(move * speed * Time.deltaTime);

                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);
            }
        }

        public void LockMovement()
        {
            lockMovement = true;
            velocity = Vector3.zero;
        }

        public void UnlockMovement()
        {
            lockMovement = false;
            velocity = Vector3.zero;
        }
    }
}
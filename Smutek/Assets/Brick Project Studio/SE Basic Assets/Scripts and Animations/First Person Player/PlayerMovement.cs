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
        public bool lockMovement;

        private Vector3 _velocity;
        private bool _isGrounded;
        private void Start()
        {
            EventManager.StartListening(UnityEvents.MOVEMENT_LOCK, LockMovement);
            EventManager.StartListening(UnityEvents.MOVEMENT_UNLOCK, UnlockMovement);
        }
        // Update is called once per frame
        void Update()
        {
            if (lockMovement) return;
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");

            var move = transform.right * x + transform.forward * z;

            controller.Move(move * (speed * Time.deltaTime));

            _velocity.y += gravity * Time.deltaTime;

            controller.Move(_velocity * Time.deltaTime);
        }

        public void LockMovement()
        {
            lockMovement = true;
            _velocity = Vector3.zero;
        }

        public void UnlockMovement()
        {
            lockMovement = false;
            _velocity = Vector3.zero;
        }
    }
}
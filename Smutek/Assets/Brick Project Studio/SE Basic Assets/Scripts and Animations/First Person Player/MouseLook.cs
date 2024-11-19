using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
    public class MouseLook : MonoBehaviour
    {

        public float mouseXSensitivity = 100f;
        public bool lockCamera;
        public Transform playerBody;

        private float _xRotation;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            EventManager.StartListening(UnityEvents.CameraLock, LockCamera);
            EventManager.StartListening(UnityEvents.CameraUnlock, UnlockCamera);
        }

        // Update is called once per frame
        void Update()
        {
            if (lockCamera) return;
            var mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * mouseXSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        public void LockCamera()
        {
            lockCamera = true;
        }

        public void UnlockCamera()
        {
            lockCamera = false;
        }
    }
}
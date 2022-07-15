using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
    public class MouseLook : MonoBehaviour
    {

        public float mouseXSensitivity = 100f;
        public bool lockCamera = false;
        public Transform playerBody;

        float xRotation = 0f;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            EventManager.StartListening(UnityEvents.CAMERA_LOCK, LockCamera);
            EventManager.StartListening(UnityEvents.CAMERA_UNLOCK, UnlockCamera);
        }

        // Update is called once per frame
        void Update()
        {
            if (!lockCamera)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseXSensitivity * Time.deltaTime;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);
            }
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
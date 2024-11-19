using System;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace SojaExiles
{
    public class PlayerMovement : MonoBehaviour
    {
        private Vector3 _velocity;
        private bool _isGrounded;
        private DynamicMoveProvider _moveProvider;
        private SnapTurnProvider _turnProvider;

        private void Awake()
        {
            _moveProvider = GameObject.FindWithTag("MoveProvider").GetComponent<DynamicMoveProvider>();
            _turnProvider = GameObject.FindWithTag("TurnProvider").GetComponent<SnapTurnProvider>();
        }

        private void Start()
        {
            EventManager.StartListening(UnityEvents.MovementLock, LockMovement);
            EventManager.StartListening(UnityEvents.MovementUnlock, UnlockMovement);
            
            _moveProvider.enabled = false;
            _turnProvider.enabled = false;
        }

        private void LockMovement()
        {
            _moveProvider.enabled = false;
            _turnProvider.enabled = false;
        }

        private void UnlockMovement()
        {
            _moveProvider.enabled = true;
            _turnProvider.enabled = true;
        }
    }
}
using Assets.Scripts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class LockMovementManager : MonoBehaviour
{
    private Vector3 _velocity;
    private bool _isGrounded;
    private DynamicMoveProvider _moveProvider;
    private SnapTurnProvider _turnProvider;
    private CharacterController _characterController;

    private void Awake()
    {
        _moveProvider = GameObject.FindWithTag("MoveProvider").GetComponent<DynamicMoveProvider>();
        _turnProvider = GameObject.FindWithTag("TurnProvider").GetComponent<SnapTurnProvider>();
        _characterController = GetComponent<CharacterController>();
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
        _characterController.height = 1.36144f;
    }
}
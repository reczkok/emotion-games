using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SplitTile : MonoBehaviour
{
    public Vector3 correctPosition;
    public PuzzleController.Direction[] mDirections = new PuzzleController.Direction[4]; //0 = UP, 1 = RIGHT, 2 = BOTTOM, 3 = LEFT
    private XRGrabInteractable interactable;
    private SpriteRenderer spriteRenderer;
    public bool isCorrect;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        interactable = GetComponent<XRGrabInteractable>();
    }

    public void OnPickUp(SelectEnterEventArgs _)
    {
        spriteRenderer.sortingOrder = 100;
    }
    
    public void OnLetGo(SelectExitEventArgs _)
    {
        spriteRenderer.sortingOrder = 0;
        var dist = (transform.localPosition - correctPosition).sqrMagnitude;
        if (!(dist < 400.0f)) return;
        transform.localPosition = correctPosition;
        spriteRenderer.sortingOrder = -100;
        interactable.enabled = false;
        isCorrect = true;
        var puzzleController = FindFirstObjectByType<PuzzleController>();
        if (puzzleController && puzzleController.HasCompleted())
        {
            puzzleController.ChangeMap();
        }
    }
}

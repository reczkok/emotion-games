using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Assets.Scripts
{
    public class FinalObjectComponent : MonoBehaviour
    {
        private DialogueTrigger trigger;
        public Dialogue OnAllItemsCollectedDialogue;
        public Dialogue finalDialogue;
        public bool finalSequenceStarted;
        private XRSimpleInteractable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<XRSimpleInteractable>();
            trigger = GetComponent<DialogueTrigger>();
        }

        public void ItemsCollected()
        {
            finalSequenceStarted = true;
            _interactable.enabled = true;
            EventManager.StartListening(UnityEvents.EndDialogueEvent, PerformFinalAction);
            CanvasPositioner.Instance.OverlayCanvasOnScreen();
            DialogueManager.Instance.StartDialogue(OnAllItemsCollectedDialogue);
        }
        
        public void HandleInteraction(Transform target)
        {
            CanvasPositioner.Instance.RepositionCanvas(target);
            trigger.TriggerDialogue();
            _interactable.enabled = false;
        }

        public void PerformFinalAction()
        {
            EventManager.StopListening(UnityEvents.EndDialogueEvent, PerformFinalAction);
            foreach (var dialogue in FindObjectsByType<DialogueTrigger>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            {
                dialogue.isUsed = true;
                dialogue.isRepeatable = false;
            }

            if (!trigger) return;
            
            trigger.dialogue = finalDialogue;
            trigger.isUsed = false;
            EventManager.StartListening(UnityEvents.EndDialogueEvent, PerformQuit);
        }

        public void PerformQuit()
        {
            SceneManager.LoadScene(1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class FinalObjectComponent : MonoBehaviour
    {
        private InteractableObject interactable;
        private DialogueTrigger trigger;
        public Dialogue OnAllItemsCollectedDialogue;
        public Dialogue finalDialogue;
        public bool finalSequenceStarted = false;
        public void Start()
        {
            interactable = GetComponent<InteractableObject>();
            trigger = GetComponent<DialogueTrigger>();
        }

        public void ItemsCollected()
        {
            EventManager.StartListening(UnityEvents.END_DIALOGUE_EVENT, PerformFinalAction);
            DialogueManager.Instance.StartDialogue(OnAllItemsCollectedDialogue);
        }

        public void PerformFinalAction()
        {
            EventManager.StopListening(UnityEvents.END_DIALOGUE_EVENT, PerformFinalAction);
            foreach (var dialogue in FindObjectsOfType<DialogueTrigger>())
            {
                dialogue.isUsed = true;
                dialogue.isRepeatable = false;
            }
            if (interactable != null && trigger != null)
            {
                interactable.isInteractable = true;
                trigger.dialogue = finalDialogue;
                trigger.isUsed = false;
                EventManager.StartListening(UnityEvents.END_DIALOGUE_EVENT, PerformQuit);
            }
        }

        public void PerformQuit()
        {
            SceneManager.LoadScene(1);
        }
    }
}

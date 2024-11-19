using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class InteractableObject: MonoBehaviour, Interactable
    {
        public bool isInteractable = true;
        public bool isCollectible;
        public bool IsInteractable()
        {
            return isInteractable;
        }

        public virtual void OnAction()
        {
            Debug.Log("Action");
            var dialog = GetComponent<DialogueTrigger>();
            if (!dialog) return;
            dialog.TriggerDialogue();
            if (isCollectible)
            {
                EventManager.StartListening(UnityEvents.EndDialogueEvent, OnCollect);
            }
        }
        
        public void MoveCanvasTo(Transform target)
        {
            CanvasPositioner.Instance.RepositionCanvas(target);
        }

        public virtual void OnCollect()
        {
            ResultCalculator.Instance.AddValue(1);
            Destroy(gameObject);
        }
    }
}

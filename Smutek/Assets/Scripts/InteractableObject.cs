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
        public bool isCollectible = false;
        public bool IsInteractable()
        {
            return isInteractable;
        }

        public virtual void OnAction()
        {
            Debug.Log("Action");
            var dialog = GetComponent<DialogueTrigger>();
            if (dialog)
            {
                dialog.TriggerDialogue();
                if (isCollectible)
                {
                    EventManager.StartListening(UnityEvents.END_DIALOGUE_EVENT, OnCollect);
                }
            }
        }

        public virtual void OnCollect()
        {
            ResultCalculator.Instance.AddValue(1);
            Destroy(gameObject);
        }

    }
}

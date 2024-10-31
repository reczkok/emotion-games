using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool isRepeatable;
    public bool isUsed;

    public void TriggerDialogue()
    {
        if(!isRepeatable && isUsed)
        {
            return;
        }
        DialogueManager.Instance.StartDialogue(dialogue);
        isUsed = true;
    }
}

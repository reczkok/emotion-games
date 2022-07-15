using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public Dialogue dialogue;
    public bool isRepeatable = false;
    public bool isUsed = false;

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

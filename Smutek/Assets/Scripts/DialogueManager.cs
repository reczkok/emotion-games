using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    private Queue<string> sentences;
    public Text dialogueText;
    public bool isOpen = false;
    public Animator animator;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DisplayNextSentence();
            }
        } 
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        isOpen = true;
        animator.SetBool("isOpen", true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
    
    void EndDialogue()
    {
        Debug.Log("End Dialogue");
        isOpen = false;
        animator.SetBool("isOpen", false);
        EventManager.TriggerEvent(UnityEvents.END_DIALOGUE_EVENT);
    }
}

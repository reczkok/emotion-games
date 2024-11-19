using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    private readonly Queue<string> _sentences = new();
    public Text dialogueText;
    public bool isOpen;
    private Canvas _canvas;
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
            _canvas = FindFirstObjectByType<Canvas>();
        }
    }


    private void Update()
    {
        if (!isOpen) return;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        _sentences.Clear();
        _canvas.enabled = true;
        foreach (var sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }
        isOpen = true;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(var letter in sentence)
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
    
    void EndDialogue()
    {
        _canvas.enabled = false;
        Debug.Log("End Dialogue");
        isOpen = false;
        EventManager.TriggerEvent(UnityEvents.EndDialogueEvent);
    }
}

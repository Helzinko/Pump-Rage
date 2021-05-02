using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> _sentences;
    public bool isTalking = false;
    public GameObject dialogHolder;
    
    public GameObject crossair;
    public CanvasGroup[] objectsToDisable;

    public TMP_Text npcName;
    public TMP_Text dialogueText;
    
    void Start()
    {
        _sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isTalking = true;

        npcName.text = dialogue.npcName;
        
        dialogHolder.SetActive(true);
        
        crossair.SetActive(false);
        Cursor.visible = true;

        foreach (var disableObject in objectsToDisable) {
            disableObject.alpha = 0f;
            disableObject.blocksRaycasts = false;
        }

        _sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        isTalking = false;
        dialogHolder.SetActive(false);
        
        crossair.SetActive(true);
        Cursor.visible = false;

        foreach (var disableObject in objectsToDisable) {
            disableObject.alpha = 1f;
            disableObject.blocksRaycasts = true;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTalking)
            {
                DisplayNextSentence();
            }
        }
    }
}

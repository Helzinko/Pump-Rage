using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueManager : MonoBehaviour
{
    private String[] _sentences;
    public bool isTalking = false;
    public GameObject dialogHolder;
    
    public GameObject crossair;
    public CanvasGroup[] objectsToDisable;

    public TMP_Text npcName;
    public TMP_Text dialogueText;

    public AudioSource talkingSound;

    private int _arrayLenght = 0;
    private int _currentSentence = 0;

    public void StartDialogue(Dialogue dialogue)
    {
        _currentSentence = 0;
        _sentences = new String[10];
        
        dialogHolder.SetActive(true);
        
        isTalking = true;

        npcName.text = dialogue.npcName;

        crossair.SetActive(false);

        foreach (var disableObject in objectsToDisable) {
            disableObject.alpha = 0f;
            disableObject.blocksRaycasts = false;
        }

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            _sentences[i] = dialogue.sentences[i];
            _arrayLenght = i;
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        talkingSound.Stop();
        
        if (_currentSentence > _arrayLenght)
        {
            EndDialogue();
            return;
        }

        string sentence = _sentences[_currentSentence];
        _currentSentence++;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        isTalking = false;
        dialogHolder.SetActive(false);
        
        crossair.SetActive(true);

        foreach (var disableObject in objectsToDisable) {
            disableObject.alpha = 1f;
            disableObject.blocksRaycasts = true;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        talkingSound.Play();
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        talkingSound.Stop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTalking && _currentSentence != 0)
            {
                DisplayNextSentence();
            }
        }
    }
}

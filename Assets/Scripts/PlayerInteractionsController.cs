using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractionsController : MonoBehaviour
{
    public GameObject exitText;

    private bool _canExit = false;

    public Animator transition;

    public GameObject pickupText;
    public GameObject startTalkText;

    private bool _canPickupUpgrade = false;
    private GameObject _upgradePickupObject;
    
    private bool _canPickupHealth = false;
    private GameObject _healthPickupObject;

    private bool _canStartTalkingWithJoe = false;

    public AudioSource openDoorSound;
    
    public AudioSource levelUpSound;
    public AudioSource healthUpSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("exit"))
        {
            if (GameObject.FindWithTag("GameController").GetComponent<GameManager>().enemiesCleared)
            {
                exitText.SetActive(true);
                _canExit = true;
            }
        }
        
        else if (other.CompareTag("upgradePickup"))
        {
            pickupText.SetActive(true);
            _canPickupUpgrade = true;
            _upgradePickupObject = other.gameObject;
        }
        
        else if (other.CompareTag("healthPickup"))
        {
            pickupText.SetActive(true);
            _canPickupHealth = true;
            _healthPickupObject = other.gameObject;
        }
        
        else if (other.CompareTag("joeNpc"))
        {
            startTalkText.SetActive(true);
            _canStartTalkingWithJoe = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("exit"))
        {
            if (GameObject.FindWithTag("GameController").GetComponent<GameManager>().enemiesCleared)
            {
                exitText.SetActive(false);
                _canExit = false;
            }
        }
        
        if (other.CompareTag("upgradePickup"))
        {
            pickupText.SetActive(false);
            _canPickupUpgrade = false;
            _upgradePickupObject = new GameObject("EmptyObject");
        }
        
        if (other.CompareTag("healthPickup"))
        {
            pickupText.SetActive(false);
            _canPickupHealth = false;
            _healthPickupObject = new GameObject("EmptyObject");
        }
        
        if (other.CompareTag("joeNpc"))
        {
            startTalkText.SetActive(false);
            _canStartTalkingWithJoe = false;

            if (FindObjectOfType<DialogueManager>().isTalking)
            {
                FindObjectOfType<DialogueManager>().EndDialogue();
            }
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_canExit)
            {
                _canExit = false;
                openDoorSound.Play();
                StartCoroutine(LoadLevel(0));
            }

            else if (_canPickupUpgrade)
            {
                levelUpSound.Play();
                GameObject.FindGameObjectWithTag("GameController").GetComponent<ShotgunLevelController>().AddUpgradePoint();
                Destroy(_upgradePickupObject);
                pickupText.SetActive(false);
                _canPickupUpgrade = false;
            }
            
            else if (_canPickupHealth)
            {
                healthUpSound.Play();
                GetComponent<PlayerStateController>().AddHealth(10);
                Destroy(_healthPickupObject);
                _canPickupHealth = false;
                pickupText.SetActive(false);
            }
            
            else if (_canStartTalkingWithJoe)
            {
                _canStartTalkingWithJoe = false;
                startTalkText.SetActive(false);
                GameObject.FindGameObjectWithTag("joeNpc").GetComponent<DialogueTrigger>().TriggerDialogue();
            }
        }
    }

    public IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("end");

        yield return new WaitForSeconds(1f);

        if (SceneManager.GetActiveScene().buildIndex == 2)
            SceneManager.LoadScene(0);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public IEnumerator RestartGame(int levelIndex)
    {
        transition.SetTrigger("end");

        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(1);
    }
}

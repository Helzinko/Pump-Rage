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

    private bool _canPickupUpgrade = false;
    private GameObject _upgradePickupObject;
    
    private bool _canPickupHealth = false;
    private GameObject _healthPickupObject;

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
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_canExit)
            {
                StartCoroutine(LoadLevel(0));
            }

            else if (_canPickupUpgrade)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<ShotgunLevelController>().AddUpgradePoint();
                Destroy(_upgradePickupObject);
                pickupText.SetActive(false);
                _canPickupUpgrade = false;
            }
            
            else if (_canPickupHealth)
            {
                GetComponent<PlayerStateController>().AddHealth(10);
                Destroy(_healthPickupObject);
                _canPickupHealth = false;
                pickupText.SetActive(false);
            }
        }
    }

    public IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("end");

        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

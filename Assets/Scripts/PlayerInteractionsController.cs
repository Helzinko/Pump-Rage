using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractionsController : MonoBehaviour
{
    public GameObject exitText;

    private bool _canExit = false;

    public Animator transition;

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
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_canExit)
            {
                StartCoroutine(LoadLevel(0));
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

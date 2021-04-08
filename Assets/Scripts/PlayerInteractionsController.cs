using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionsController : MonoBehaviour
{
    public GameObject exitText;

    private bool _canExit = false;
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
            if(_canExit)
                Debug.Log("Go");
        }
    }
}

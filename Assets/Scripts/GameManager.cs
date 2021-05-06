using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool visibleMouse = false; 
    
    private GameObject _player;
    private PlayerStateController _stateController;

    private int enemyCount;

    public bool enemiesCleared = false;

    void Start()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("enemy");
        enemyCount = allEnemies.Length;

        Cursor.visible = visibleMouse;

        _player = GameObject.FindGameObjectWithTag("Player");
        _stateController = _player.GetComponent<PlayerStateController>();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if(currentSceneIndex != 0)
            GameObject.FindGameObjectWithTag("soundtrack").GetComponent<SoundtrackController>().PlayMusic();
        else GameObject.FindGameObjectWithTag("soundtrack").GetComponent<SoundtrackController>().StopMusic();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_stateController.isDead)
            {
                StartCoroutine(_player.GetComponent<PlayerInteractionsController>().RestartGame(0));
            }
        }

    }

    public void enemyCalculator(int enemyValue)
    {
        enemyCount--;
        GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().SetCurrentScore(enemyValue);

        if (enemyCount <= 0)
        {
            enemiesCleared = true;
        }
        
    }
}

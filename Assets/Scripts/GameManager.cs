using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        
        GameObject.FindGameObjectWithTag("soundtrack").GetComponent<SoundtrackController>().PlayMusic();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_stateController.isDead)
            {
                StartCoroutine(_player.GetComponent<PlayerInteractionsController>().LoadLevel(0));
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

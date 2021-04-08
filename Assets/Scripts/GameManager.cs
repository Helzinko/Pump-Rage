﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool visibleMouse = false; 
    
    private GameObject _player;
    private PlayerStateController _stateController;

    void Start()
    {
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
                Application.LoadLevel(Application.loadedLevel);
        }

    }
}
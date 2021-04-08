using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables : MonoBehaviour
{
    public float _playerHealth = 100f;
    public int _currentBulletCount = 4;
    
    

    private static Variables _variablesInstance;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        if (_variablesInstance == null) {
            _variablesInstance = this;
        } else {
            DestroyObject(gameObject);
        }
    }

    public void RestoreDefault()
    {
        _playerHealth = 100f;
        _currentBulletCount = 4;
    }

    public float GetPlayerHealth()
    {
        return _playerHealth;
    }

    public void SetPlayerHealth(float newHealth)
    {
        _playerHealth = newHealth;
    }

    public int GetCurrentBulletCount()
    {
        return _currentBulletCount;
    }

    public void SetCurrentBulletCount(int newBulletCount)
    {
        _currentBulletCount = newBulletCount;
    }
    
}

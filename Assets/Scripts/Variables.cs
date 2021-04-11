using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables : MonoBehaviour
{
    public float playerHealth = 100f;
    public int currentBulletCount = 4;

    public int currentShotgunLevel = 1;
    public float currentShotgunXp = 0;
    public int currentNextLevelXp = 100;

    public int currentUpgradePoints = 0;
    
    public int currentBulletRangeLevel = 0;
    public int currentBulletDamageLevel = 0;
    public int currentBulletCountLevel = 0;

    public int currentScore = 0;
    
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
        playerHealth = 100f;
        currentBulletCount = 4;
        currentShotgunLevel = 1;
        currentShotgunXp = 0;
        currentNextLevelXp = 100;
        currentUpgradePoints = 0;
        
        currentBulletRangeLevel = 0; 
        currentBulletDamageLevel = 0;
        currentBulletCountLevel = 0;
    }

    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    public void SetPlayerHealth(float newHealth)
    {
        playerHealth = newHealth;
    }

    public int GetCurrentBulletCount()
    {
        return currentBulletCount;
    }

    public void SetCurrentBulletCount(int newBulletCount)
    {
        currentBulletCount = newBulletCount;
    }

    public int GetCurrentShotgunLevel()
    {
        return currentShotgunLevel;
    }

    public void SetCurrentShotgunLevel(int newShotgunLevel)
    {
        currentShotgunLevel = newShotgunLevel;
    }

    public float GetCurrentShotgunXp()
    {
        return currentShotgunXp;
    }

    public void SetCurrentShotgunXp(float newShotgunXp)
    {
        currentShotgunXp = newShotgunXp;
    }

    public int GetCurrentNextLevelXp()
    {
        return currentNextLevelXp;
    }

    public void SetCurrentNextLevelXp(int newNextLevelXp)
    {
        currentNextLevelXp = newNextLevelXp;
    }
    
    public int GetCurrentUpgradePoints()
    {
        return currentUpgradePoints;
    }

    public void SetCurrentUpgradePoints(int newUpgradePoints)
    {
        currentUpgradePoints = newUpgradePoints;
    }
    
    public int GetCurrentBulletDamageLevel()
    {
        return currentBulletDamageLevel;
    }

    public void SetCurrentBulletDamageLevel(int newBulletDamageLevel)
    {
        currentBulletDamageLevel = newBulletDamageLevel;
    }
    
    public int GetBulletRangeLevel()
    {
        return currentBulletRangeLevel;
    }

    public void SetCurrentBulletRangeLevel(int newBulletRangeLevel)
    {
        currentBulletRangeLevel = newBulletRangeLevel;
    }
    
    public int GetCurrentBulletCountLevel()
    {
        return currentBulletCountLevel;
    }

    public void SetCurrentBulletCountLevel(int newBulletCountLevel)
    {
        currentBulletCountLevel = newBulletCountLevel;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void SetCurrentScore(int newScore)
    {
        currentScore += newScore;
    }
    
}

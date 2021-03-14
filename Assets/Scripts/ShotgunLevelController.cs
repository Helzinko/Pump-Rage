using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunLevelController : MonoBehaviour
{
    public float currentXp = 0;
    private int _nextLevelXp = 100;
    public int currentLevel = 1;

    private void Start()
    {
        GetComponent<ShotgunBarController>().ChangeLevelBarValue(currentXp/_nextLevelXp);
    }

    public void GetXp(float xp)
    {
        currentXp += xp;
        GetComponent<ShotgunBarController>().ChangeLevelBarValue(currentXp/_nextLevelXp);
        GetComponent<ShotgunUpgradeController>().AddExperiencePoints(currentXp/_nextLevelXp);

        if (currentXp >= _nextLevelXp)
            LevelUp();
    }

    private void LevelUp()
    {
        currentXp = currentXp - _nextLevelXp;
        currentLevel++;
        _nextLevelXp += _nextLevelXp / 10;
        GetComponent<ShotgunBarController>().ChangeLevelValue(currentLevel);
        GetComponent<ShotgunBarController>().ChangeLevelBarValue(currentXp/_nextLevelXp);
        
        GetComponent<ShotgunUpgradeController>().AddUpgradePoint(1);
        GetComponent<ShotgunUpgradeController>().AddExperiencePoints(currentXp/_nextLevelXp);
        GetComponent<ShotgunUpgradeController>().AddLevel(currentLevel);
    }
}

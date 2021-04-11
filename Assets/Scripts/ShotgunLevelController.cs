using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShotgunLevelController : MonoBehaviour
{
    public float currentXp = 0;
    private int _nextLevelXp = 100;
    public int currentLevel = 1;

    public GameObject upgradeAddedText;

    private void Start()
    {
        GetComponent<ShotgunBarController>().ChangeLevelBarValue(currentXp/_nextLevelXp);
        
        currentLevel = GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().GetCurrentShotgunLevel();
        currentXp = GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().GetCurrentShotgunXp();
        _nextLevelXp = GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().GetCurrentNextLevelXp();
        GetComponent<ShotgunUpgradeController>().AddExperiencePoints(currentXp/_nextLevelXp);
        GetComponent<ShotgunUpgradeController>().AddLevel(currentLevel);
            
        GetComponent<ShotgunBarController>().ChangeLevelValue(currentLevel);
        GetComponent<ShotgunBarController>().ChangeLevelBarValue(currentXp/_nextLevelXp);
    }

    public void GetXp(float xp)
    {
        currentXp += xp;
        GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().SetCurrentShotgunXp(currentXp);
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
        
        GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().SetCurrentShotgunXp(currentXp);
        GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().SetCurrentShotgunLevel(currentLevel);
        GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().SetCurrentNextLevelXp(_nextLevelXp);
        
        GetComponent<ShotgunBarController>().ChangeLevelValue(currentLevel);
        GetComponent<ShotgunBarController>().ChangeLevelBarValue(currentXp/_nextLevelXp);
        
        GetComponent<ShotgunUpgradeController>().AddExperiencePoints(currentXp/_nextLevelXp);
        GetComponent<ShotgunUpgradeController>().AddLevel(currentLevel);

        AddUpgradePoint();
    }

    public void AddUpgradePoint()
    {
        GetComponent<ShotgunUpgradeController>().AddUpgradePoint(1);
        upgradeAddedText.SetActive(true);
        Invoke("DisableUpgradeAddedText", 2.5f);
    }

    private void DisableUpgradeAddedText()
    {
        upgradeAddedText.SetActive(false);
    }
}

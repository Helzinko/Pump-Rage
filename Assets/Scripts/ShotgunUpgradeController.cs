﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunUpgradeController : MonoBehaviour
{
    private int _upgradePoints = 0;
    public TMP_Text upgradePointsText;
    
    private float _currentXp = 0;
    public Image experiencePointsFillImage;

    private int _currentLevel = 1;
    public TMP_Text currentLevelText;
    
    public CanvasGroup upgradeTable;
    public GameObject crossair;
    public CanvasGroup[] objectsToDisable;

    public bool isUpgrading = false;
    
    // special upgrades
    private int _bulletRangeLevel = 0;
    public TMP_Text bulletRangeLevelText;
    public float bulletLifeTime = 0.10f;

    private int _bulletDamageLevel = 0;
    public TMP_Text bulletDamageLevelText;
    public float bulletDamage = 1;
    
    private int _bulletCountLevel = 0;
    public TMP_Text bulletCountLevelText;

    private void Start()
    {
        upgradeTable.alpha = 0f;
        upgradeTable.blocksRaycasts = false;
    }

    public void ExitUpgradeBtn()
    {
        ExitController();
    }
    
    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!isUpgrading)
            {
                upgradeTable.alpha = 1f;
                upgradeTable.blocksRaycasts = true;
                
                crossair.SetActive(false);
                Cursor.visible = true;

                foreach (var disableObject in objectsToDisable) {
                    disableObject.alpha = 0f;
                    disableObject.blocksRaycasts = false;
                }

                isUpgrading = true;
            }
            else
            {
                ExitController();
            }
 
        }
    }

    public void ExitController()
    {
        upgradeTable.alpha = 0f;
        upgradeTable.blocksRaycasts = false;

        crossair.SetActive(true);
        Cursor.visible = false;

        foreach (var disableObject in objectsToDisable) {
            disableObject.alpha = 1f;
            disableObject.blocksRaycasts = true;
        }
                
        isUpgrading = false;
    }

    public void AddUpgradePoint(int amount)
    {
        _upgradePoints += amount;
        upgradePointsText.text = _upgradePoints.ToString();
        Console.WriteLine(upgradePointsText.text);
    }

    public void AddExperiencePoints(float amount)
    {
        _currentXp = amount;
        experiencePointsFillImage.fillAmount = _currentXp;
    }

    public void AddLevel(int currentLevel)
    {
        _currentLevel = currentLevel;
        currentLevelText.text = _currentLevel.ToString();
    }

    
    // upgrades
    
    public void UpgradeBulletRange()
    {
        if (_upgradePoints > 0)
        {
            bulletLifeTime += 0.02f;
            _bulletRangeLevel++;
            bulletRangeLevelText.text = _bulletRangeLevel.ToString();
            AddUpgradePoint(-1);
        }

    }
    
    public void UpgradeBulletDamage()
    {
        if (_upgradePoints > 0)
        {
            bulletDamage += 0.5f;
            _bulletDamageLevel++;
            bulletDamageLevelText.text = _bulletDamageLevel.ToString();
            AddUpgradePoint(-1);
        }
    }
    
    public void UpgradeBulletCount()
    {
        if (_upgradePoints > 0)
        {
            GetComponent<ShotgunBarController>().shotgun.GetComponent<ShotgunController>()._maxBulletCount++;
            GetComponent<ShotgunBarController>().ChangeBulletsText(GetComponent<ShotgunBarController>().shotgun.GetComponent<ShotgunController>().currentBulletsCount);
            _bulletCountLevel++;
            bulletCountLevelText.text = _bulletCountLevel.ToString();
            AddUpgradePoint(-1);
        }
    }
}
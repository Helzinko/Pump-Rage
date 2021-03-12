using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunUpgradeController : MonoBehaviour
{
    private int _upgradePoints = 0;
    public GameObject upgradeTable;
    public GameObject crossair;
    public CanvasGroup[] objectsToDisable;

    public bool isUpgrading = false;

    private void Start()
    {
        upgradeTable.SetActive(false);
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
                upgradeTable.SetActive(true);
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
        upgradeTable.SetActive(false);
        crossair.SetActive(true);
        Cursor.visible = false;

        foreach (var disableObject in objectsToDisable) {
            disableObject.alpha = 1f;
            disableObject.blocksRaycasts = true;
        }
                
        isUpgrading = false;
    }
}

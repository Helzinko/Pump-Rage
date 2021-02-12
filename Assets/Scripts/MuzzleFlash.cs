using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject[] flashHolder;

    private int currentFlashIndex;
    
    public float flashTime;

    public void Activate()
    {
        currentFlashIndex = UnityEngine.Random.Range(0, flashHolder.Length);
        flashHolder[currentFlashIndex].SetActive(true);
        Invoke("Deactivate", flashTime);
    }

    void Deactivate()
    {
        flashHolder[currentFlashIndex].SetActive(false);
    }
}

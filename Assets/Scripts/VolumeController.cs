using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Text volumeCount;
    
    private float currentFloatValue;
    void Start()
    {
        currentFloatValue = PlayerPrefs.GetFloat("Volume", 0.75f);
        
        audioMixer.SetFloat(audioMixer.name, Mathf.Log10(currentFloatValue) * 20);
        volumeCount.text = Mathf.Round(currentFloatValue * 100).ToString();
    }

    public void PlusSound()
    {
        if (currentFloatValue >= 1f)
            return;
        
        currentFloatValue += 0.05f;
        audioMixer.SetFloat(audioMixer.name, Mathf.Log10(currentFloatValue) * 20);
        volumeCount.text = Mathf.Round(currentFloatValue * 100).ToString();
        
        PlayerPrefs.SetFloat("Volume", currentFloatValue);
    }

    public void MinusSound()
    {
        if (currentFloatValue <= 0)
            return;
        
        currentFloatValue -= 0.05f;
        audioMixer.SetFloat(audioMixer.name, Mathf.Log10(currentFloatValue) * 20);
        volumeCount.text = Mathf.Round(currentFloatValue * 100).ToString();
        
        PlayerPrefs.SetFloat("Volume", currentFloatValue);
    }
}

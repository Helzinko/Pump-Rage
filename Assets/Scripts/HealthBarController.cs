using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private float _playerHealth;
    
    public Image healthImage;
    public Image fillImage;
    public Image numberBackgroundImage;

    public Sprite[] characterSprites;

    private Color _fillImageColor;

    public Color blinkColor;

    public TMP_Text healthCount;
    public TMP_Text rageCount;

    private float _currentHealth = 100;
    private void Start()
    {
        _fillImageColor = fillImage.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            fillImage.fillAmount -= 0.10f;
            _currentHealth = fillImage.fillAmount * 100;
            healthCount.text = Mathf.Round(_currentHealth).ToString();
            ChangeHealthSprite(fillImage.fillAmount);
            StartCoroutine("HealthFillBlink");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            fillImage.fillAmount += 0.10f;
            _currentHealth = fillImage.fillAmount * 100;
            healthCount.text = Mathf.Round(_currentHealth).ToString();
            ChangeHealthSprite(fillImage.fillAmount);
        }
    }

    private void ChangeHealthSprite(float fillAmount)
    {
        if (fillAmount > 0.8f)
        {
            rageCount.text = "1";
            healthImage.sprite = characterSprites[0];
        }
        else if (fillAmount < 0.8f && fillAmount > 0.6f)
        {
            rageCount.text = "2";
            healthImage.sprite = characterSprites[1];
        }
        else if (fillAmount < 0.6f && fillAmount > 0.4f)
        {
            rageCount.text = "3";
            healthImage.sprite = characterSprites[2];
        }
        else if (fillAmount < 0.4f && fillAmount > 0f)
        {
            rageCount.text = "4";
            healthImage.sprite = characterSprites[3];
        }
        else if (fillAmount <= 0f)
        {
            rageCount.text = "0";
            healthImage.sprite = characterSprites[4];
        }
    }

    IEnumerator HealthFillBlink()
    {
        fillImage.color = blinkColor;
        numberBackgroundImage.color = blinkColor;
        yield return new WaitForSeconds(0.1f);
        fillImage.color = _fillImageColor;
        numberBackgroundImage.color = _fillImageColor;
    }
}

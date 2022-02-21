using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public bool isDead = false;
    public float playerHealth;
    public GameObject healthBarController;

    public GameObject splashEffect;
    public GameObject[] decalObjects;

    private Animator _animator;

    public GameObject deathText;
    
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    public GameObject healthAddedText;

    public AudioSource gettingDamageSound;
    public AudioSource dieSound;

    private int _currentScoreTemp;
    private int _highscoreTemp;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        //playerHealth = GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().GetPlayerHealth();
        playerHealth = 10;
        //healthBarController.GetComponent<HealthBarController>().ChangeBarData(playerHealth);
       
    }
 

    private void ShowDeathText()
    {
        deathText.SetActive(true);
        scoreText.text = "Neutralized infected persons: <color=red>" + _currentScoreTemp + "</color>";
        highScoreText.text = "Highscore: <color=red>" + _highscoreTemp + "</color>";
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead)
            return;
        
        gettingDamageSound.Play();
        
        playerHealth -= damageAmount;
        GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().SetPlayerHealth(playerHealth);
        
        if (playerHealth <= 0)
        {
            _currentScoreTemp = GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().GetCurrentScore();
            _highscoreTemp = PlayerPrefs.GetInt("highScore", 0);
            
            if (_currentScoreTemp > _highscoreTemp)
            {
                _highscoreTemp = _currentScoreTemp;
                PlayerPrefs.SetInt("highScore", _highscoreTemp);
            }
            
            dieSound.Play();
            isDead = true;
            GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().RestoreDefault();
            Invoke("ShowDeathText", 2f);
            
            if (FindObjectOfType<DialogueManager>().isTalking)
            {
                FindObjectOfType<DialogueManager>().EndDialogue();
            }
        }

        healthBarController.GetComponent<HealthBarController>().ChangeBarData(playerHealth);
        
        Quaternion playerSplashRotation = Quaternion.Euler(-90f, 0, 0);
        Vector3 playerSplashPosition = new Vector3(transform.position.x, 2, transform.position.z);
        GameObject playerSplashEffect = Instantiate(splashEffect, playerSplashPosition, playerSplashRotation);
        Destroy(playerSplashEffect, 1f);
        Vector3 decalPosition = new Vector3(transform.position.x, -0.8f, transform.position.z);
        Quaternion decalRotation = Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z);
        GameObject decalGameObject = Instantiate(decalObjects[UnityEngine.Random.Range(0, decalObjects.Length)], decalPosition, decalRotation);
    }

    public void AddHealth(float healthAmount)
    {
        if (playerHealth >= 90)
            playerHealth = 100;

        playerHealth += healthAmount;
        
        GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().SetPlayerHealth(playerHealth);
        healthBarController.GetComponent<HealthBarController>().ChangeBarData(playerHealth);
        
        healthAddedText.SetActive(true);
        Invoke("DisableHealthAddedText", 2.5f);
    }
    
    private void DisableHealthAddedText()
    {
        healthAddedText.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        playerHealth = GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().GetPlayerHealth();
        healthBarController.GetComponent<HealthBarController>().ChangeBarData(playerHealth);
    }

    private void ShowDeathText()
    {
        deathText.SetActive(true);
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead)
            return;
        
        playerHealth -= damageAmount;
        GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().SetPlayerHealth(playerHealth);
        
        if (playerHealth <= 0)
        {
            _animator.SetTrigger("Die");
            isDead = true;
            GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().RestoreDefault();
            Invoke("ShowDeathText", 2f);
        }

        healthBarController.GetComponent<HealthBarController>().ChangeBarData(playerHealth);
        
        Quaternion playerSplashRotation = Quaternion.Euler(-90f, 0, 0);
        Vector3 playerSplashPosition = new Vector3(transform.position.x, 2, transform.position.z);
        GameObject playerSplashEffect = Instantiate(splashEffect, playerSplashPosition, playerSplashRotation);
        Destroy(playerSplashEffect, 1f);
        Vector3 decalPosition = new Vector3(transform.position.x, -0.3f, transform.position.z);
        Quaternion decalRotation = Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z);
        GameObject decalGameObject = Instantiate(decalObjects[UnityEngine.Random.Range(0, decalObjects.Length)], decalPosition, decalRotation);
    }
}

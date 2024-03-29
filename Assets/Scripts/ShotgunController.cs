﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : MonoBehaviour
{
    public Transform muzzle;
    public Bullet bullet;
    public float timeBetweenFire = 100;
    public float bulletSpeed = 35;

    private float _nextFireTime;

    private Animator _animator;

    private GameObject _player;

    public Transform shell;
    public Transform shellSpawn;

    private MuzzleFlash _muzzleFlash;

    public Transform[] BulletSpawns;

    public bool multipleShot = false;

    public CameraShake cameraShake;
    
    private PlayerStateController _stateController;
    
    public int currentBulletsCount;
    public int _maxBulletCount = 7;

    public AudioSource shotSound;
    public AudioSource reloadSound;
    public AudioSource emptyShot;

    public GameObject gameManager;

    private bool _isReloading = false;

    private void Start()
    {
        //currentBulletsCount = GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>()
        //    .GetCurrentBulletCount();

        currentBulletsCount = 7;

        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _muzzleFlash = GetComponent<MuzzleFlash>();
        _stateController = _player.GetComponent<PlayerStateController>();
        gameManager.GetComponent<ShotgunBarController>().ChangeBulletsText(currentBulletsCount);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !_player.GetComponent<MovementController>().isDashing && !_stateController.isDead && !_isReloading && !FindObjectOfType<ShotgunUpgradeController>().isUpgrading)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && !_player.GetComponent<MovementController>().isDashing && !_stateController.isDead)
        {
            if (currentBulletsCount != _maxBulletCount && !_isReloading)
            {
                _isReloading = true;
                
                // reload animation
            
                reloadSound.Play();
            
                //
                currentBulletsCount = _maxBulletCount;
                gameManager.GetComponent<ShotgunBarController>().ChangeBulletsText(currentBulletsCount);
                GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().SetCurrentBulletCount(currentBulletsCount);
                
                Invoke("EndReloading", 0.75f);
            }
        }
    }

    private void EndReloading()
    {
        _isReloading = false;
    }

    private void Shoot()
    {
        if (Time.time > _nextFireTime)
        {
            if (currentBulletsCount > 0)
            {
                multipleShot = false;
                //StartCoroutine(cameraShake.Shake(.15f, .1f));
                _nextFireTime = Time.time + timeBetweenFire / 1000;

                TripleShoot();
    
                _muzzleFlash.Activate();
                Instantiate(shell, shellSpawn.position, shellSpawn.rotation);
            
                _animator.SetTrigger("shoot");
                currentBulletsCount--;
                gameManager.GetComponent<ShotgunBarController>().ChangeBulletsText(currentBulletsCount);
                GameObject.FindGameObjectWithTag("variables").GetComponent<Variables>().SetCurrentBulletCount(currentBulletsCount);
                shotSound.Play();
            }
            else
            {            
                emptyShot.Play();
                _nextFireTime = Time.time + timeBetweenFire / 1000;
            }
        }
    }

    private void TripleShoot()
    {
        for (int i = 0; i < BulletSpawns.Length; i++)
        {
            Bullet newBullet = Instantiate(bullet, muzzle.position, BulletSpawns[i].rotation);
            newBullet.SetBulletSpeed(bulletSpeed);
        }
    }
}
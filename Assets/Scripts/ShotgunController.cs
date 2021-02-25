using System;
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

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _muzzleFlash = GetComponent<MuzzleFlash>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !_player.GetComponent<MovementController>().isDashing)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time > _nextFireTime)
        {
            multipleShot = false;
            StartCoroutine(cameraShake.Shake(.15f, .1f));
            _nextFireTime = Time.time + timeBetweenFire / 1000;

            TripleShoot();
    
            _muzzleFlash.Activate();
            Instantiate(shell, shellSpawn.position, shellSpawn.rotation);
            
            _animator.SetTrigger("shoot");
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
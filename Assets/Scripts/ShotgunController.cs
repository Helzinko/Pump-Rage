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

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _muzzleFlash = GetComponent<MuzzleFlash>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
        
        transform.LookAt(new Vector3(0, 0, 0));
    }

    private void Shoot()
    {
        if (Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + timeBetweenFire / 1000;
            Bullet newBullet = Instantiate(bullet, muzzle.position, _player.transform.rotation);
            newBullet.SetBulletSpeed(bulletSpeed);
    
            _muzzleFlash.Activate();
            Instantiate(shell, shellSpawn.position, shellSpawn.rotation);
            
            _animator.SetTrigger("shoot");
        }
    }
}

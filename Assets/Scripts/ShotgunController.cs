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

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + timeBetweenFire / 1000;
            Bullet newBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as Bullet;
            newBullet.SetBulletSpeed(bulletSpeed);
        }
    }
}

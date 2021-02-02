using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed = 10;

    public void SetBulletSpeed(float speed)
    {
        _speed = speed;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * _speed));
    }

    private void Start()
    {
        Destroy(gameObject, 2.0f);
    }
}

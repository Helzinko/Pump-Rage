using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed = 10;
    public LayerMask collisionMask;
    private float _damage = 1;

    public void SetBulletSpeed(float speed)
    {
        _speed = speed;
    }

    private void Update()
    {
        float moveDistance = _speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    private void CheckCollisions(float moveDistance)
    {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(_damage, hit);
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        Destroy(gameObject, 2.0f);
    }
}

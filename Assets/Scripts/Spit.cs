using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    private float _speed = 10;
    public LayerMask collisionMask;
    private float _damage = 20;
    public GameObject spitSplashEffect;

    public void SetSpitSpeed(float speed)
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
        if (hit.transform.CompareTag("Player"))
        {
            if(hit.collider != null)
                hit.collider.GetComponent<PlayerStateController>().TakeDamage(_damage);
        }
        
        GameObject playerSplashEffect = Instantiate(spitSplashEffect, transform.position, transform.rotation);
        Destroy(playerSplashEffect, 1f);
        
        Vector3 decalPosition = new Vector3(transform.position.x, -0.3f, transform.position.z);
        Quaternion decalRotation = Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z);

        Destroy(gameObject);
    }

    private void Start()
    {
        Destroy(gameObject, 1f);
        transform.LookAt(GameObject.FindWithTag("enemyTarget").transform);
    }
}
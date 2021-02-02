using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    private NavMeshAgent _navMeshAgent;
    private Transform _player;

    public float _health = 3.0f;
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Update()
    {
        _navMeshAgent.SetDestination(_player.position);
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

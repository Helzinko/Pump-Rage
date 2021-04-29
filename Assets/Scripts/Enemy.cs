using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking,
        Stunned,
        Dead
    }

    public State _currentState;
    
    public NavMeshAgent _navMeshAgent;
    private Transform _player;

    private float _smellDistance = 25f;

    public bool smelledPlayer = false;

    private Animator _animator;

    public float _health = 3.0f;

    public GameObject splashEffect;
    public GameObject[] decalObjects;

    private List<Collider> RagdollColliders = new List<Collider>();
    private List<Rigidbody> RagdollRigidbodies = new List<Rigidbody>();

    public float xpValue = 10;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("playerBody").transform;
        SetRagdollParts();
    }

    private void SetRagdollParts()
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        Rigidbody[] rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();

        foreach (Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                c.gameObject.layer = 16;
                c.isTrigger = true;
                RagdollColliders.Add(c);
            }
        }
        
        foreach (Rigidbody c in rigidbodies)
        {
            if (c.gameObject != this.gameObject)
            {
                c.isKinematic = true;
                RagdollRigidbodies.Add(c);
            }
        }
    }
    
    private void TurnOnRagdoll()
    {
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        _animator.enabled = false;
        
        foreach (Rigidbody c in RagdollRigidbodies)
        {
            c.isKinematic = false;
            Vector3 hitDirection = _player.transform.position - transform.position;
            c.AddForce(-hitDirection.normalized * 100f, ForceMode.Impulse);
        }
        
        foreach (Collider c in RagdollColliders)
        {
            c.isTrigger = false;
        }
    }

    private void Update()
    {
        if (_currentState == State.Dead)
            return;
        
        if (_currentState == State.Chasing)
        {
            _navMeshAgent.SetDestination(_player.position);
        }

        if (Vector3.Distance(_player.position, transform.position) <= _smellDistance && !smelledPlayer)
        {
            if (_currentState != State.Stunned)
            {
                smelledPlayer = true;
                _currentState = State.Chasing;
                _animator.SetBool("chase", true);
            }
        }
    }
    
    public void TakeHit(float damage, RaycastHit hit)
    {
        if (_currentState == State.Dead)
            return;
        
        _currentState = State.Stunned;
        _navMeshAgent.enabled = false;

        _health -= damage;
        
        if (_health <= 0)
        {
            Die();
            TurnOnRagdoll();
            return;
        }
        
        Invoke("RemoveStun", .1f);
        var particlesSpawnVector = new Vector3(transform.position.x, 1f, transform.position.z);
        GameObject splashParticles = Instantiate(splashEffect, particlesSpawnVector, transform.rotation);
        Vector3 decalPosition = new Vector3(transform.position.x, -0.8f, transform.position.z);
        Quaternion decalRotation = Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z);
        GameObject decalGameObject = Instantiate(decalObjects[UnityEngine.Random.Range(0, decalObjects.Length)], decalPosition, decalRotation);
        Destroy(splashParticles, 1f);
    }

    public void RemoveStun()
    {
        if (_currentState == State.Dead)
            return;
        
        _navMeshAgent.enabled = true;

        _currentState = State.Chasing;
        
        smelledPlayer = true;
        _animator.SetBool("chase", true);
    }

    private void Die()
    {
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().enemyCalculator(1);
        _navMeshAgent.enabled = false;
        var particlesSpawnVector = new Vector3(transform.position.x, 1f, transform.position.z);
        GameObject splashParticles = Instantiate(splashEffect, particlesSpawnVector, transform.rotation);
        Vector3 decalPosition = new Vector3(transform.position.x, -0.8f, transform.position.z);
        Quaternion decalRotation = Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z);
        GameObject decalGameObject = Instantiate(decalObjects[UnityEngine.Random.Range(0, decalObjects.Length)], decalPosition, decalRotation);
        Destroy(splashParticles, 1f);
        _currentState = State.Dead;
        
        
        GameObject gameManager = GameObject.FindWithTag("GameController");
        gameManager.GetComponent<ShotgunLevelController>().GetXp(xpValue);
    }
}

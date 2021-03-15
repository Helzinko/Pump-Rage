using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class RangeEnemy : MonoBehaviour, IDamageable
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking,
        Stunned,
        Dead
    }

    private State _currentState;
    
    private NavMeshAgent _navMeshAgent;
    private Transform _player;

    private float _attackDistance = 5f;
    private float _timeBetweenAttacks = 1;
    private float _nextAttackTime;

    private float _smellDistance = 30f;

    private bool smelledPlayer = false;

    private Animator _animator;

    private Rigidbody _rigidbody;

    public float _health = 3.0f;

    private GameObject _shotgun;

    public GameObject splashEffect;
    public GameObject[] decalObjects;

    public float xpValue = 10;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("playerBody").transform;
        _shotgun = GameObject.FindGameObjectWithTag("shotgun");
        _rigidbody = GetComponent<Rigidbody>();
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
            smelledPlayer = true;
            _currentState = State.Chasing;
            _animator.SetBool("chase", true);
        }
    }
    
    private IEnumerator Attack()
    {
        _animator.SetBool("chase", false);
        _currentState = State.Attacking;
        yield return new WaitForSeconds(1f);
    }

    void FixedUpdate()
    {
        if (_currentState == State.Dead)
            return;

        if (Time.time > _nextAttackTime)
        {
            float sqrDistanceToPlayer = (_player.position - transform.position).sqrMagnitude;
            if (sqrDistanceToPlayer < Mathf.Pow(_attackDistance, 2))
            {
                _nextAttackTime = Time.time + _timeBetweenAttacks;
                transform.LookAt(_player);
                _animator.SetTrigger("attack");
                StartCoroutine("Attack");
            }
            else
            {
                if (_currentState != State.Chasing && smelledPlayer)
                {
                    _animator.SetBool("chase", true);
                    _currentState = State.Chasing;
                }
            }
        }
    }
    
    public void TakeHit(float damage, RaycastHit hit)
    {
        if (_currentState == State.Dead)
            return;
        
        _currentState = State.Stunned;
        
        _health -= damage;

        if (!_shotgun.GetComponent<ShotgunController>().multipleShot)
        {
            _shotgun.GetComponent<ShotgunController>().multipleShot = true;
            Vector3 hitDirection = _player.transform.position - transform.position;
            _rigidbody.AddForce(-hitDirection.normalized * 15f, ForceMode.Impulse);
        }
        
        if (_health <= 0)
        {
            Die();
            return;
        }
        
        Invoke("RemoveStun", .5f);
        GameObject enemySplashEffect = Instantiate(splashEffect, transform.position, transform.rotation);

        Vector3 decalPosition = new Vector3(transform.position.x, -0.3f, transform.position.z);
        Quaternion decalRotation = Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z);
        GameObject decalGameObject = Instantiate(decalObjects[UnityEngine.Random.Range(0, decalObjects.Length)], decalPosition, decalRotation);
        Destroy(enemySplashEffect, 1f);
    }

    public void RemoveStun()
    {
        if (_currentState == State.Dead)
            return;
        
        _currentState = State.Chasing;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        if (!smelledPlayer)
        {
            smelledPlayer = true;
            _animator.SetBool("chase", true);
        }
    }

    private void Die()
    {
        _navMeshAgent.enabled = false;
        GameObject enemySplashEffect = Instantiate(splashEffect, transform.position, transform.rotation);
        Vector3 decalPosition = new Vector3(transform.position.x, -0.3f, transform.position.z);
        Quaternion decalRotation = Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z);
        GameObject decalGameObject = Instantiate(decalObjects[UnityEngine.Random.Range(0, decalObjects.Length)], decalPosition, decalRotation);
        Destroy(enemySplashEffect, 1f);
        _currentState = State.Dead;
        _animator.SetTrigger("die");
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        
        // +xp
        GameObject gameManager = GameObject.FindWithTag("GameController");
        gameManager.GetComponent<ShotgunLevelController>().GetXp(xpValue);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent (typeof (NavMeshAgent))]
public class FirstBossController : MonoBehaviour, IDamageable
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking,
        Stunned,
        Dead,
        Taunt
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

    private NavMeshPath _navMeshPath;

    public float xpValue = 10;

    public AudioSource zombieRoarSound;
    public AudioSource hurtEffect;
    public AudioSource dieSound;
    public AudioSource tauntSound;
    
    public float _attackDistance = 5f;
    private float _timeBetweenAttacks = 1;
    private float _nextAttackTime;

    public Image fillImage;

    private float _maxHealth;

    private bool _changedState = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("playerBody").transform;
        _navMeshPath = new NavMeshPath();
        SetRagdollParts();
        _maxHealth = _health;
        fillImage.fillAmount = _health / _maxHealth;
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
    
    private IEnumerator Attack()
    {
        if(!_changedState)
            _animator.SetBool("chase", false);
        else _animator.SetBool("angry_chase", false);
        
        _currentState = State.Attacking;
        yield return new WaitForSeconds(0.6f);
    }

    void FixedUpdate()
    {
        if (_currentState == State.Dead)
            return;
        
        if (_currentState == State.Taunt)
            return;

        if (Time.time > _nextAttackTime)
        {
            float sqrDistanceToPlayer = (_player.position - transform.position).sqrMagnitude;
            if (sqrDistanceToPlayer < Mathf.Pow(_attackDistance, 2))
            {
                _nextAttackTime = Time.time + _timeBetweenAttacks;
                transform.LookAt(_player);
                
                if(!_changedState)
                    _animator.SetTrigger("attack");
                else _animator.SetTrigger("angry_attack");
                
                StartCoroutine("Attack");
            }
            else
            {
                if (_currentState != State.Chasing && smelledPlayer)
                {
                    if (_currentState != State.Stunned)
                    {
                       
                        if(!_changedState)
                            _animator.SetBool("chase", true);
                        else _animator.SetBool("angry_chase", true);
                        
                        _currentState = State.Chasing;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (_currentState == State.Taunt)
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
                zombieRoarSound.Play();
                _currentState = State.Chasing;
                
                if(!_changedState)
                    _animator.SetBool("chase", true);
                else _animator.SetBool("angry_chase", true);
            }
        }
    }

    private void RemoveTaunt()
    {
        _currentState = State.Chasing;
        _navMeshAgent.enabled = true;
        _animator.SetBool("angry_chase", true);
    }
    
    public void TakeHit(float damage, RaycastHit hit)
    {
        if (_currentState == State.Dead)
            return;
        
        _health -= damage;
        fillImage.fillAmount = _health / _maxHealth;

        if (_health <= _maxHealth / 2 && !_changedState)
        {
            tauntSound.Play();
            _changedState = true;
            _animator.SetTrigger("taunt");
            _navMeshAgent.speed = 20;
            _currentState = State.Taunt;
            _navMeshAgent.enabled = false;
            Invoke("RemoveTaunt", 1.5f);
        }
        
        if (_health <= 0)
        {
            Die();
            TurnOnRagdoll();
            return;
        }
        
        hurtEffect.Play();
        
        var particlesSpawnVector = new Vector3(transform.position.x, 1f, transform.position.z);
        GameObject splashParticles = Instantiate(splashEffect, particlesSpawnVector, transform.rotation);
        Vector3 decalPosition = new Vector3(transform.position.x, -0.8f, transform.position.z);
        Quaternion decalRotation = Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z);
        GameObject decalGameObject = Instantiate(decalObjects[UnityEngine.Random.Range(0, decalObjects.Length)], decalPosition, decalRotation);
        Destroy(splashParticles, 1f);
    }
    
    private void Die()
    {
        var dieSoundHolder = Instantiate(dieSound, transform);
        dieSoundHolder.transform.parent = null;
        
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

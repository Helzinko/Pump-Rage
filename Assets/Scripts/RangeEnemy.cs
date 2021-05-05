using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    private float _attackDistance = 15f;
    private float _timeBetweenAttacks = 1;
    private float _nextAttackTime;
    private Animator _animator;

    private Transform _player;

    private Enemy enemyScript;

    public Spit spit;
    public Transform spitSpawn;

    public AudioSource spitSound;

    void Start()
    {
        _animator = GetComponent<Animator>();
        enemyScript = GetComponent<Enemy>();
        _player = GameObject.FindGameObjectWithTag("playerBody").transform;
    }

    private IEnumerator Attack()
    {
        enemyScript._navMeshAgent.isStopped = true;
        _animator.SetBool("chase", false);
        enemyScript._currentState = Enemy.State.Attacking;
        
        Spit newSpit = Instantiate(spit, spitSpawn.position, spitSpawn.rotation);
        newSpit.SetSpitSpeed(70f);

        yield return new WaitForSeconds(1f);

        if (enemyScript._currentState != Enemy.State.Dead)
            if (enemyScript._currentState != Enemy.State.Stunned)
                enemyScript._navMeshAgent.isStopped = false;
    }

    void FixedUpdate()
    {
        if (enemyScript._currentState == Enemy.State.Dead)
            return;

        //if (enemyScript._currentState != Enemy.State.Idle)
            //transform.LookAt(_player);

        if (Time.time > _nextAttackTime)
        {
            float sqrDistanceToPlayer = (_player.position - transform.position).sqrMagnitude;
            if (sqrDistanceToPlayer < Mathf.Pow(_attackDistance, 2))
            {
                RaycastHit hit;
                var rayDirection = _player.position - transform.position;
                if (Physics.Raycast(transform.position, rayDirection, out hit))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        spitSound.Play();
                        _nextAttackTime = Time.time + _timeBetweenAttacks;
                        _animator.SetTrigger("attack");
                        StartCoroutine("Attack");
                    }
                    else
                    {
                        if (enemyScript._currentState != Enemy.State.Stunned)
                        {
                            _animator.SetBool("chase", true);
                            enemyScript._currentState = Enemy.State.Chasing;
                        }
                    }
                }
            }
            else
            {
                if (enemyScript._currentState != Enemy.State.Chasing && enemyScript.smelledPlayer)
                {
                    if (enemyScript._currentState != Enemy.State.Stunned)
                    {
                        _animator.SetBool("chase", true);
                        enemyScript._currentState = Enemy.State.Chasing;
                    }
                }
            }
        }
    }
}
    
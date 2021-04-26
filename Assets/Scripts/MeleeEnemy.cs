using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private float _attackDistance = 5f;
    private float _timeBetweenAttacks = 1;
    private float _nextAttackTime;
    public GameObject punchObject;
    private Animator _animator;
    
    private Transform _player;

    private Enemy enemyScript;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        enemyScript = GetComponent<Enemy>();
        punchObject.SetActive(false);
        _player = GameObject.FindGameObjectWithTag("playerBody").transform;
    }
    private IEnumerator Attack()
    {
        punchObject.SetActive(true);
        _animator.SetBool("chase", false);
        enemyScript._currentState = Enemy.State.Attacking;
        yield return new WaitForSeconds(1.1f);
        punchObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (enemyScript._currentState == Enemy.State.Dead)
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

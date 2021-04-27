using UnityEngine;

public class BombEnemy : MonoBehaviour
{
    private float _attackDistance = 6f;
    private Animator _animator;
    
    private Transform _player;

    private Enemy enemyScript;
    
    public float _damage = 40;
    
    public GameObject splashEffect;
    void Start()
    {
        _animator = GetComponent<Animator>();
        enemyScript = GetComponent<Enemy>();
        _player = GameObject.FindGameObjectWithTag("playerBody").transform;
    }

    void FixedUpdate()
    {
        if (enemyScript._currentState == Enemy.State.Dead)
            return;

        float sqrDistanceToPlayer = (_player.position - transform.position).sqrMagnitude;
        if (sqrDistanceToPlayer < Mathf.Pow(_attackDistance, 2))
        {
            var particlesSpawnVector = new Vector3(transform.position.x, 1f, transform.position.z);
            GameObject splashParticles = Instantiate(splashEffect, particlesSpawnVector, transform.rotation);
            Destroy(splashParticles, 1f);
            
            sqrDistanceToPlayer = (_player.position - transform.position).sqrMagnitude;
            if (sqrDistanceToPlayer < Mathf.Pow(_attackDistance, 2))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateController>().TakeDamage(_damage);
            }
            
            Destroy(gameObject);
        }
    }
}
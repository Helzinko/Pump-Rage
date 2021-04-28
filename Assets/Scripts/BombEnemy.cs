using UnityEngine;

public class BombEnemy : MonoBehaviour
{
    private float _attackDistance = 6f;

    private Transform _player;

    private Enemy enemyScript;
    
    public float _damage = 40;

    public GameObject splashEffect;

    private GameObject _playerScript;
    void Start()
    {
        enemyScript = GetComponent<Enemy>();
        _player = GameObject.FindGameObjectWithTag("playerBody").transform;
        _playerScript = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (enemyScript._currentState == Enemy.State.Dead)
            return;

        float sqrDistanceToPlayer = (_player.position - transform.position).sqrMagnitude;
        if (sqrDistanceToPlayer < Mathf.Pow(_attackDistance, 2))
        {
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().enemyCalculator(1);
            var particlesSpawnVector = new Vector3(transform.position.x, 1f, transform.position.z);
            GameObject splashParticles = Instantiate(splashEffect, particlesSpawnVector, transform.rotation);
            Destroy(splashParticles, 1f);

            var decalObjects = GetComponent<Enemy>().decalObjects;
            
            Vector3 decalPosition = new Vector3(transform.position.x, -0.8f, transform.position.z);
            Quaternion decalRotation = Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z);
            GameObject decalGameObject = Instantiate(decalObjects[UnityEngine.Random.Range(0, decalObjects.Length)], decalPosition, decalRotation);
            
            sqrDistanceToPlayer = (_player.position - transform.position).sqrMagnitude;
            if (sqrDistanceToPlayer < Mathf.Pow(_attackDistance, 2))
            {
                _playerScript.GetComponent<PlayerStateController>().TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
    }
}
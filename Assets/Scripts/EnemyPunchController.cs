using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPunchController : MonoBehaviour
{
    public float punchDamage = 10f;
    private GameObject _player;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.GetComponent<PlayerStateController>().TakeDamage(punchDamage);
            //gameObject.SetActive(false);
        }
    }
}

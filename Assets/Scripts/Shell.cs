using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{

    private Rigidbody _rigidbody;
    public float forceMin;
    public float forceMax;

    private float _lifeTime = 3.5f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        float force = Random.Range(forceMin, forceMax);
        _rigidbody.AddForce(transform.right * force);
        _rigidbody.AddTorque(Random.insideUnitSphere * force);

        Destroy(gameObject, _lifeTime);
    }
    
}

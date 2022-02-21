using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _offset;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _offset = transform.position - _player.transform.position;
    }

    private void Update()
    {
        var position = transform.position;
        var followPoint = new Vector3(_player.transform.position.x+_offset.x, position.y, _player.transform.position.z+_offset.z);
        position = Vector3.Lerp(position, followPoint, 0.1f);
        transform.position = position;
    }
}

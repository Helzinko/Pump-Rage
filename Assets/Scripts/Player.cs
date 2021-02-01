using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 5;
    private Vector3 _velocity;
    private Rigidbody _rigidbody;
    private Vector3 _playerVelocity;

    private Camera _camera;

    private Plane _groundObject;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
        _groundObject = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        _playerVelocity = (new Vector3(Input.GetAxisRaw("Horizontal"), 
            0, Input.GetAxisRaw("Vertical"))).normalized * movementSpeed;

        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!_groundObject.Raycast(ray, out var rayDistance)) return;
        
        var point = ray.GetPoint(rayDistance);
        var lookPoint = new Vector3(point.x, transform.position.y, point.z);
        transform.LookAt(lookPoint);
    }
    
    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _playerVelocity * Time.fixedDeltaTime);
    }
}

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

    private Animator _animator;
    private static readonly int Turn = Animator.StringToHash("Turn");
    private static readonly int Forward = Animator.StringToHash("Forward");

    private Transform _cam;
    private Vector3 _camForward;
    private Vector3 _move;
    private Vector3 _moveInput;
    private float _forwardAmount;
    private float _turnAmount;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
        _groundObject = new Plane(Vector3.up, Vector3.zero);

        _animator = GetComponent<Animator>();
        _cam = _camera.transform;
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (_cam != null)
        {
            _camForward = Vector3.Scale(_cam.up, new Vector3(1, 0, 1)).normalized;
            _move = vertical * _camForward + horizontal * _cam.right;
        }
        else
        {
            _move = vertical * Vector3.forward + horizontal * Vector3.right;
        }

        if (_move.magnitude > 1)
        {
            _move.Normalize();
        }

        Move(_move);
        
        _playerVelocity = (new Vector3(horizontal, 
            0, vertical)).normalized * movementSpeed;

        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!_groundObject.Raycast(ray, out var rayDistance)) return;
        
        var point = ray.GetPoint(rayDistance);
        var lookPoint = new Vector3(point.x, transform.position.y, point.z);
        transform.LookAt(lookPoint);
    }

    private void Move(Vector3 move)
    {
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        this._moveInput = move;

        ConvertMoveInput();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        _animator.SetFloat(Turn, _forwardAmount, 0.1f, Time.deltaTime);
        _animator.SetFloat(Forward, _turnAmount, 0.1f, Time.deltaTime);
    }

    private void ConvertMoveInput()
    {
        var localMove = transform.InverseTransformDirection(_moveInput);
        _turnAmount = localMove.x;
        _forwardAmount = localMove.z;
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _playerVelocity * Time.fixedDeltaTime);
    }
}

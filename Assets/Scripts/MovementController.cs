using UnityEngine;

public class MovementController : MonoBehaviour
{
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

    public GameObject crosshair;

    public GameObject shotgun;

    public bool isDashing = false;

    private PlayerStateController stateController;

    public AudioSource walkingSound;
    public AudioSource dashSound;

    private CharacterController _controller;
    public float _playerMovementSpeed = 5;

    private Vector3 inputVector;

    private bool _walkingSoundActivated = false;
    void Start()
    {
        _camera = Camera.main;
        _groundObject = new Plane(Vector3.up, Vector3.up * shotgun.transform.position.y);

        _animator = GetComponent<Animator>();
        _cam = _camera.transform;
        stateController = GetComponent<PlayerStateController>();

        inputVector = Vector2.zero;
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _controller.SimpleMove(inputVector * _playerMovementSpeed);

        if (_playerVelocity != Vector3.zero)
        {
            if (!stateController.isDead)
            {
                if (!isDashing)
                {
                    if (!_walkingSoundActivated)
                    {
                        walkingSound.Play();
                        _walkingSoundActivated = true;
                    }
                }
            }
        }
        else
        {
            walkingSound.Stop();
            _walkingSoundActivated = false;
        }

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

        if(!stateController.isDead)
            Move(_move);
        
        _playerVelocity = (new Vector3(horizontal, 
            0, vertical)).normalized * _playerMovementSpeed;

        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!_groundObject.Raycast(ray, out var rayDistance)) return;
        
        var point = ray.GetPoint(rayDistance);
        var lookPoint = new Vector3(point.x, transform.position.y, point.z);
        
        if(!isDashing && !stateController.isDead)
            transform.LookAt(lookPoint);

        //crosshair.transform.position = point;
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
        //if(!isDashing && !stateController.isDead)
        //    _rigidbody.MovePosition(_rigidbody.position + _playerVelocity * Time.fixedDeltaTime);
    }
}

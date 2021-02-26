using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
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

    public GameObject crosshair;

    public GameObject shotgun;

    public bool isDashing = false;

    private PlayerStateController stateController;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
        _groundObject = new Plane(Vector3.up, Vector3.up * shotgun.transform.position.y);

        _animator = GetComponent<Animator>();
        _cam = _camera.transform;
        stateController = GetComponent<PlayerStateController>();
    }

    void RemoveDashForce()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        isDashing = false;
    }

    void Dash(Vector3 force)
    {
        _rigidbody.AddForce(force * 1200f * Time.fixedDeltaTime, ForceMode.VelocityChange);
        isDashing = true;
        _animator.SetTrigger("Roll");
        Invoke("RemoveDashForce", 0.4665f);
    }

    void Update()
    {
        if (!isDashing && !stateController.isDead)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.rotation = Quaternion.AngleAxis(45, Vector3.up);
                        Dash(new Vector3(0.75f, 0, 0.75f));
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        transform.rotation = Quaternion.AngleAxis(-45, Vector3.up);
                        Dash(new Vector3(-0.75f, 0, 0.75f));
                    }
                    else
                    {
                        transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                        Dash(Vector3.forward);
                    }
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.rotation = Quaternion.AngleAxis(135, Vector3.up);
                        Dash(new Vector3(0.75f, 0, -0.75f));
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        transform.rotation = Quaternion.AngleAxis(225, Vector3.up);
                        Dash(new Vector3(-0.75f, 0, -0.75f));
                    }
                    else
                    {
                        transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                        Dash(Vector3.back);
                    }
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                    Dash(Vector3.right);
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
                    Dash(Vector3.left);
                }
            }
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
            0, vertical)).normalized * movementSpeed;

        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!_groundObject.Raycast(ray, out var rayDistance)) return;
        
        var point = ray.GetPoint(rayDistance);
        var lookPoint = new Vector3(point.x, transform.position.y, point.z);
        
        if(!isDashing && !stateController.isDead)
            transform.LookAt(lookPoint);
        crosshair.transform.position = point;
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
        if(!isDashing && !stateController.isDead)
            _rigidbody.MovePosition(_rigidbody.position + _playerVelocity * Time.fixedDeltaTime);
    }
}

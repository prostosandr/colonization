using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Ground _ground;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _zoomSpeed;

    [SerializeField] private float _xOffset;
    [SerializeField] private float _zOffset;
    [SerializeField] private float _minHeightOffset;
    [SerializeField] private float _maxHeightOffset;

    private PlayerInput _playerInput;
    private Vector2 _moveDirection;
    private Vector2 _rotateDirection;
    private Vector2 _zoomDirection;

    private Bounds _bounds;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void Start()
    {
        _bounds = _ground.Bounds;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        _moveDirection = _playerInput.Camera.Move.ReadValue<Vector2>();
        _rotateDirection = _playerInput.Camera.Rotate.ReadValue<Vector2>();
        _zoomDirection = _playerInput.Camera.Scroll.ReadValue<Vector2>();

        Move();
        Rotate();
        Zoom();
    }

    private void Move()
    {
        if (_moveDirection.sqrMagnitude < 0.1f)
            return;

        float scaledMoveSpeed = _moveSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(_moveDirection.x, 0f, _moveDirection.y) * scaledMoveSpeed;

        transform.Translate(offset);

        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, _bounds.min.x + _xOffset, _bounds.max.x - _xOffset);
        newPosition.z = Mathf.Clamp(newPosition.z, _bounds.min.z + _xOffset, _bounds.max.z - _xOffset);

        transform.position = newPosition;
    }

    private void Rotate()
    {
        if (_rotateDirection.sqrMagnitude < 0.1f)
            return;

        float scaledRotateSpeed = _rotateSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(0f, _rotateDirection.x, 0f) * scaledRotateSpeed;

        transform.Rotate(offset);
    }

    private void Zoom()
    {
        if (_zoomDirection.sqrMagnitude < 0.1f)
            return;

        float scaledZoomSpeed = _zoomSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(0f, _zoomDirection.y, 0f) * scaledZoomSpeed;

        transform.Translate(offset);

        Vector3 newPosition = transform.position;
        newPosition.y = Mathf.Clamp(newPosition.y, _ground.transform.position.y + _minHeightOffset, _ground.transform.position.y + _maxHeightOffset);

        transform.position = newPosition;
    }
}
using UnityEngine;

public class Joystick : MonoBehaviour
{
    [SerializeField] private Transform _innerJoystickTransform;

    private const float InnerJoystickMaxMagnitude = 75f;

    private Transform _transform;
    private Vector3 _startPosition;
    private Vector3 _innerJoystickPosition;

    void Awake()
    {
        _transform = transform;
        _startPosition = transform.position;

        PlayerInput.OnMouseClicked += CenterJoystickToClickPosition;
        PlayerInput.OnMouseDragged += SetInnerJoystickPosition;
        PlayerInput.OnMouseUnclicked += SetJoystickToStartPosition;
    }

    void Start()
    {
        SetJoystickToStartPosition();
    }

    private void CenterJoystickToClickPosition(Vector3 position)
    {
        _transform.position = position;
        _innerJoystickPosition = position;
    }

    private void SetInnerJoystickPosition(Vector3 direction)
    {
        direction = Vector3.ClampMagnitude(direction, InnerJoystickMaxMagnitude);
        _innerJoystickPosition = _transform.position + direction;
    }

    private void SetJoystickToStartPosition()
    {
        _transform.position = _startPosition;
        _innerJoystickPosition = _startPosition;
    }

    void Update()
    {
        _innerJoystickTransform.position = _innerJoystickPosition;
    }

    void OnDestroy()
    {
        PlayerInput.OnMouseClicked -= CenterJoystickToClickPosition;
        PlayerInput.OnMouseDragged -= SetInnerJoystickPosition;
        PlayerInput.OnMouseUnclicked -= SetJoystickToStartPosition;
    }
}
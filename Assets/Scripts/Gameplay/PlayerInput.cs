using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    public static Action<Vector3> OnMouseClicked;
    public static Action<Vector3> OnMouseDragged;
    public static Action OnMouseUnclicked;

    private const int InputLayerMask = 1 << 8;

    private Camera _mainCamera;
    private Vector3 _direction;
    private Vector3 _clickPosition;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.HasGameStarted) return;

        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse) &&
            Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, InputLayerMask))
        {
            _clickPosition = Input.mousePosition;
            OnMouseClicked(_clickPosition);
        }

        if (Input.GetMouseButton((int)MouseButton.LeftMouse))
        {
            _direction = (Input.mousePosition - _clickPosition);
            
            if (_direction != Vector3.zero)
            {
                OnMouseDragged?.Invoke(_direction);
            }
        }

        if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse))
        {
            _clickPosition = Vector3.zero;
            OnMouseUnclicked?.Invoke();
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCamera : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _distance = 4f;
    [SerializeField] float _height = 1.5f;
    [SerializeField] float _mouseSensitivity = 2f;
    [SerializeField] Vector2 _verticalLimit = new Vector2(-40f, 65f);
    [SerializeField] PlayerInput _playerInput;

    InputAction _lookAction;
    Transform _tr;
    Vector2 _look;
    Vector3 _targetPos, _camPos;
    Quaternion _rot;
    float _yaw;
    float _pitch;
    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _lookAction = _playerInput.actions["Look"];
    }
    private void OnEnable()
    {
        _lookAction?.Enable();
    }

    private void OnDisable()
    {
        _lookAction?.Disable();
    }
    /// <summary>
    /// ÉJÉÅÉâì¸óÕ
    /// </summary>
    public void CameraInput()
    {
        _look = _lookAction.ReadValue<Vector2>() * _mouseSensitivity;

        _yaw += _look.x;
        _pitch -= _look.y;
        _pitch = Mathf.Clamp(_pitch, _verticalLimit.x, _verticalLimit.y);
    }
    /// <summary>
    /// ÉJÉÅÉâí«è]
    /// </summary>
    public void CameraFollow()
    {
        _rot = Quaternion.Euler(_pitch, _yaw, 0);
        _targetPos = _target.position + Vector3.up * _height;
        _camPos = _targetPos - _rot * Vector3.forward * _distance;

        _tr.position = _camPos;
        _tr.LookAt(_targetPos);
    }
}

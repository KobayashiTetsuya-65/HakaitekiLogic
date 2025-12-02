using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] float _jumpPower = 5f;
    [SerializeField] float _sprintMultiplier = 1.8f;
    [SerializeField] float _speedLerpRate = 8f;
    [SerializeField] float _groundCheckRadius = 0.25f;
    [SerializeField] float _groundCheckDistance = 0.3f;
    [SerializeField] Transform _cameraTr;
    [SerializeField] LayerMask _groundLayer;

    PlayerInput _playerInput;
    InputAction _moveAction,_jumpAction,_sprintAction;
    Rigidbody _rb;
    Quaternion _targetRot;
    Vector3 _moveInput,_move,_targetVel,_origin;
    bool _isSprinting = false;
    float _sprintFactor = 1f, _targetFactor;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _sprintAction = _playerInput.actions["Sprint"];
    }
    private void OnEnable()
    {
        _moveAction?.Enable();
        _jumpAction?.Enable();
        _sprintAction?.Enable();
    }

    private void OnDisable()
    {
        _moveAction?.Disable();
        _jumpAction?.Disable();
        _sprintAction?.Disable();
    }
    /// <summary>
    /// プレイヤー入力
    /// </summary>
    public void PlayerInput()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        _isSprinting = _sprintAction.IsPressed();
        if (_jumpAction.WasPressedThisFrame() && GroundChecker())
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
    }
    /// <summary>
    /// プレイヤー挙動
    /// </summary>
    public void PlayerMove()
    {
        _move = _cameraTr.forward * _moveInput.y + _cameraTr.right * _moveInput.x;
        _move.y = 0f;

        _targetFactor = _isSprinting ? _sprintMultiplier : 1f;
        _sprintFactor = Mathf.Lerp(_sprintFactor, _targetFactor, Time.deltaTime * _speedLerpRate);

        _targetVel = _move.normalized * (_speed * _sprintFactor);
        _rb.linearVelocity = new Vector3(_targetVel.x, _rb.linearVelocity.y, _targetVel.z);
        if (_move != Vector3.zero)
        {
            _targetRot = Quaternion.LookRotation(_move);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, _targetRot, 0.15f));
        }
    }
    /// <summary>
    /// 接地判定
    /// </summary>
    private bool GroundChecker()
    {
        _origin = transform.position + Vector3.up * (_groundCheckRadius + 0.05f);
        return Physics.SphereCast(
            _origin,
            _groundCheckRadius,
            Vector3.down,
            out _,
            _groundCheckDistance,
            _groundLayer
        );
    }
}

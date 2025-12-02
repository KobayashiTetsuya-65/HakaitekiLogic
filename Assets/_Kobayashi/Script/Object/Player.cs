using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] float _jumpPower = 5f;
    [SerializeField] float _groundCheckRadius = 0.25f;
    [SerializeField] float _groundCheckDistance = 0.3f;
    [SerializeField] Transform _cameraTr;
    [SerializeField] LayerMask _groundLayer;

    PlayerInput _playerInput;
    InputAction _moveAction,_jumpAction;
    Rigidbody _rb;
    Vector3 _moveInput,_move,_targetVel;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
    }
    private void OnEnable()
    {
        _moveAction?.Enable();
        _jumpAction?.Enable();
    }

    private void OnDisable()
    {
        _moveAction?.Disable();
        _jumpAction?.Disable();
    }
    /// <summary>
    /// プレイヤー入力
    /// </summary>
    public void PlayerInput()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
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

        _targetVel = _move.normalized * _speed ;
        _rb.linearVelocity = new Vector3(_targetVel.x,_rb.angularVelocity.y,_targetVel.z);
    }
    /// <summary>
    /// 接地判定
    /// </summary>
    public bool GroundChecker()
    {
        return Physics.SphereCast(
            transform.position + Vector3.up * 0.1f,
            _groundCheckRadius,
            Vector3.down,
            out _,
            _groundCheckDistance,
            _groundLayer
        );
    }
}

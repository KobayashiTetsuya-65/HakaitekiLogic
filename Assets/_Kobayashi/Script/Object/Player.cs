using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("数値設定")]
    [SerializeField] float _speed = 3f;
    [SerializeField] float _jumpPower = 5f;
    [SerializeField] float _sprintMultiplier = 1.8f;
    [SerializeField] float _speedLerpRate = 8f;
    [SerializeField] float _groundCheckRadius = 0.25f;
    [SerializeField] float _groundCheckDistance = 0.3f;
    [SerializeField] float _fallCheckVelocity = -2f;
    [SerializeField] float _delayJump = 0.2f;
    [SerializeField] float _landingCheckDistance = 0.6f;
    [SerializeField] float _throwDuration = 2f;

    [Header("コンポーネント設定")]
    [SerializeField] Transform _cameraTr;
    [SerializeField] LayerMask _groundLayer;

    Animator _anim;
    PlayerInput _playerInput;
    InputAction _moveAction,_jumpAction,_sprintAction,_throwAction;
    Transform _tr;
    Rigidbody _rb;
    Quaternion _targetRot;
    Vector3 _moveInput,_move,_targetVel,_origin;
    bool _isSprinting = false, _isFalling = false, _isJumping = false,_isGround = false,_isLanding = false,_isThrow = false;
    float _sprintFactor = 1f, _targetFactor, _horizontalSpeed;
    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _sprintAction = _playerInput.actions["Sprint"];
        _throwAction = _playerInput.actions["Interact"];
        _targetRot = _tr.rotation;
    }
    private void OnEnable()
    {
        _moveAction?.Enable();
        _jumpAction?.Enable();
        _sprintAction?.Enable();
        _throwAction?.Enable();
    }

    private void OnDisable()
    {
        _moveAction?.Disable();
        _jumpAction?.Disable();
        _sprintAction?.Disable();
        _throwAction?.Disable();
    }
    /// <summary>
    /// プレイヤー入力
    /// </summary>
    public void PlayerInput()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        _isSprinting = _sprintAction.IsPressed();
        if (_jumpAction.WasPressedThisFrame() && _isGround && !_isJumping && !_isThrow)
        {
            _isJumping = true;
            StartCoroutine(Jump());
        }
        if(_throwAction.WasPressedThisFrame() && !_isJumping && !_isThrow && _isGround)
        {
            _isThrow = true;
            GameManager.Instance.Move = false;
            StartCoroutine(ThrowBomb());
        }
    }
    /// <summary>
    /// プレイヤー挙動
    /// </summary>
    public void PlayerMove()
    {
        _isGround = GroundChecker();
        _move = _cameraTr.forward * _moveInput.y + _cameraTr.right * _moveInput.x;
        _move.y = 0f;

        _targetFactor = _isSprinting ? _sprintMultiplier : 1f;
        _sprintFactor = Mathf.Lerp(_sprintFactor, _targetFactor, Time.deltaTime * _speedLerpRate);

        _targetVel = _move.normalized * (_speed * _sprintFactor);
        _rb.linearVelocity = new Vector3(_targetVel.x, _rb.linearVelocity.y, _targetVel.z);
        if (_moveInput.sqrMagnitude > 0.01f)
        {
            _targetRot = Quaternion.LookRotation(_move);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, _targetRot, 0.15f));
        }

        UpdateAnimator();
    }
    /// <summary>
    /// アニメーター動かす
    /// </summary>
    private void UpdateAnimator()
    {
        _horizontalSpeed = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z).magnitude;
        _isFalling = !_isGround && _rb.linearVelocity.y < _fallCheckVelocity;
        _isLanding = _isFalling && Physics.Raycast(transform.position, Vector3.down, _landingCheckDistance, _groundLayer);

        _anim.SetBool("IsLanding", _isLanding);
        _anim.SetFloat("Speed", _horizontalSpeed, 0.1f,Time.deltaTime);
        _anim.SetBool("IsGrounded", _isGround);
        _anim.SetBool("IsFalling", _isFalling);
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
    /// <summary>
    /// ジャンプ
    /// </summary>
    /// <returns></returns>
    private IEnumerator Jump()
    {
        _anim.SetTrigger("Jump");
        yield return new WaitForSeconds(_delayJump);
        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        yield return new WaitUntil(() => _isGround);
        yield return new WaitForSeconds(0.05f);
        _isJumping = false;
    }
    /// <summary>
    /// 爆弾を投げる
    /// </summary>
    /// <returns></returns>
    private IEnumerator ThrowBomb()
    {
        _anim.SetTrigger("Throw");
        yield return new WaitForSeconds(_throwDuration);
        _isThrow = false;
        GameManager.Instance.Move = true;
    }
}

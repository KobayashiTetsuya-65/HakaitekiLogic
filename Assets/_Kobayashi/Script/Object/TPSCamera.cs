using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCamera : MonoBehaviour
{
    [SerializeField] Transform _target;

    [Header("êîílê›íË")]
    [SerializeField] float _distance = 10f;
    [SerializeField] float _height = 10f;
    [SerializeField] float _rotateX = 50f;
    
    Transform _tr;
    Vector3 _targetPos, _camPos,_distancePos;

    private void Awake()
    {
        _tr = transform;
        _tr.localRotation = Quaternion.Euler(_rotateX,0f,0f);
    }

    /// <summary>
    /// ÉJÉÅÉâí«è]
    /// </summary>
    public void CameraFollow()
    {
        _targetPos = new Vector3(_target.position.x,_height,_target.position.z);
        _camPos = _targetPos - _tr.forward * _distance;

        _tr.position = _camPos;
        _tr.LookAt(_targetPos);
    }
}

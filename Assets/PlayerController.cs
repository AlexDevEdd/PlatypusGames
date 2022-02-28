using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveVerticalSpeed = 20f;
    [SerializeField] private float _moveHorizontalSpeed = 20f;

    private int _runHash = Animator.StringToHash("IsRun");
    private int _idleHash = Animator.StringToHash("IsIdle");

    private Rigidbody _rigidbody;
    private VariableJoystick _joystick;
    private Animator _animator;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _joystick = FindObjectOfType<VariableJoystick>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
          _rigidbody.velocity = (new Vector3(_joystick.Horizontal * _moveHorizontalSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveVerticalSpeed));

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            _animator.SetBool(_runHash, true);
            _animator.SetBool(_idleHash, false);          
        }
        else
        {
            _animator.SetBool(_idleHash, true);
            _animator.SetBool(_runHash, false);         
        }
    }

}


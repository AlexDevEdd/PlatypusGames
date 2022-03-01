using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _moveVerticalSpeed = 20f;
    [SerializeField] private float _moveHorizontalSpeed = 20f;

    private Rigidbody _rigidbody;
    private VariableJoystick _joystick;
    private PlayerMovement _playerController;
    private AnimationController _animationController;


    private Dictionary<Type, IPlayerBehavior> _behaviorsMap;
    private IPlayerBehavior _behaviorCurrent;

    public float MoveVerticalSpeed => _moveVerticalSpeed;
    public float MoveHorizontalSpeed => _moveHorizontalSpeed;

    public void Awake()
    {
        _animationController = GetComponent<AnimationController>();
        _rigidbody = GetComponent<Rigidbody>();
        _joystick = FindObjectOfType<VariableJoystick>();
        _playerController = GetComponent<PlayerMovement>();
        InitBehaviors();
        SetBehaviorByDefault();
    }

    private void FixedUpdate()
    {
        if (_behaviorCurrent != null)
            _behaviorCurrent.FixedUpdate();
    }
    private void InitBehaviors()
    {
        _behaviorsMap = new Dictionary<Type, IPlayerBehavior>();

        _behaviorsMap[typeof(PlayerBehaviorWalking)] = new PlayerBehaviorWalking(this, _rigidbody, _joystick, _playerController, _animationController);
        _behaviorsMap[typeof(PlayerBehaviorIdle)] = new PlayerBehaviorIdle(this, _joystick, _animationController);

    }
    private void SetBehavior(IPlayerBehavior newBehavior)
    {
        if (_behaviorCurrent != null)
            _behaviorCurrent.Exit();
        if (_behaviorCurrent == newBehavior)
            return;

        _behaviorCurrent = newBehavior;
        _behaviorCurrent.Enter();
    }
    private void SetBehaviorByDefault()
    {
        SetBehaviorIdle();
    }
    private IPlayerBehavior GetBehavior<T>() where T : IPlayerBehavior
    {
        var type = typeof(T);
        return _behaviorsMap[type];
    }
    public void SetBehaviorIdle()
    {
        var behaviorIdle = GetBehavior<PlayerBehaviorIdle>();
        SetBehavior(behaviorIdle);
    }
    public void SetBehaviorWalking()
    {
        var behaviorWalking = GetBehavior<PlayerBehaviorWalking>();
        SetBehavior(behaviorWalking);
    }
   

  
}


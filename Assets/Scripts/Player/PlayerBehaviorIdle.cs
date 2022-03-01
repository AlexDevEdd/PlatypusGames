using System;
using UnityEngine;

public class PlayerBehaviorIdle : IPlayerBehavior
{
    private Player _player;
    private VariableJoystick _joystick;
    private AnimationController _animation;

    public PlayerBehaviorIdle(Player player, VariableJoystick joystick, AnimationController animation)
    {
        _player = player;
        _joystick = joystick;
        _animation = animation;
    }
    public void Enter()
    {
        _animation.SetIdleAnimation();
    }

    public void Exit()
    {
        _animation.SetWalkingAnimation();
    }

    public void FixedUpdate()
    {
        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            _player.SetBehaviorWalking();             
    }
}


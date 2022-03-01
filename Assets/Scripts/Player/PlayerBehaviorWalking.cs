using UnityEngine;
public class PlayerBehaviorWalking : IPlayerBehavior
{
    private Player _player;
    private Rigidbody _rigidbody;
    private VariableJoystick _joystick;
    private PlayerMovement _playerController;
    private AnimationController _animation;

    public PlayerBehaviorWalking(Player player, Rigidbody rigidbody,
        VariableJoystick joystick, PlayerMovement playerController, AnimationController animation)
    {
        _player = player;
        _rigidbody = rigidbody;
        _joystick = joystick;
        _playerController = playerController;
        _animation = animation;
    }

    public void Enter()
    {
        _animation.SetWalkingAnimation();
    }
    public void Exit()
    {
        _animation.SetIdleAnimation();
    }

    public void FixedUpdate()
    {
        Vector3 direction = _playerController.PlayerMove(_rigidbody, _joystick, _player.MoveVerticalSpeed, _player.MoveHorizontalSpeed);

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            _playerController.LookAtPlayer(_player.transform, direction);
        else
            _player.SetBehaviorIdle();

    }
}
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 PlayerMove(Rigidbody rigidbody, VariableJoystick joystick, float verticalSpeed, float horizontalSpeed  )
    {
        rigidbody.velocity = (new Vector3(joystick.Horizontal * horizontalSpeed, rigidbody.velocity.y, joystick.Vertical * verticalSpeed));
        return rigidbody.velocity;
    }

    public void LookAtPlayer(Transform transform, Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }
}


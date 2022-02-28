using UnityEngine;

public class ObjectInteraction
{
   private Rigidbody _objectRigidbody;
   private Transform _target;
   private Rigidbody _toPoint;
   private SphereCollider _sphereCollider;

    public ObjectInteraction(Rigidbody objectRigidbody, Transform target, Rigidbody toPoint, SphereCollider sphereCollider)
    {
        _objectRigidbody = objectRigidbody;
        _target = target;
        _toPoint = toPoint;
        _sphereCollider = sphereCollider;
    }

    public void Drag()
    {
        _target.position = _toPoint.position;
        _target.rotation = _toPoint.rotation;    
    }

    public void Drop(float forceY = 20f, float forceZ = 20f) =>
         _objectRigidbody.AddRelativeForce(new Vector3(0, 1 * forceY, 1 * forceZ));
    
    public void AddFixedJoint(Transform target, Rigidbody toPoint) =>
        target.gameObject.AddComponent<FixedJoint>().connectedBody = toPoint;
    
    public void EnableInteractionCollider() =>
        _sphereCollider.enabled = true;
    public void DisableInteractionCollider() =>
        _sphereCollider.enabled = false;
}

using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Item : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string BOX_TAG = "Box";
    private const string BOX_FLOOR_TAG = "BoxFloor";
    private const string GROUND_TAG = "Ground";

    [SerializeField] private IntReactiveProperty _scoreValue = new IntReactiveProperty(100);
    [SerializeField] private float _forceZ = 20f;
    [SerializeField] private float _forceY = 20f;

    private ArmTag _arm;
    private ObjectInteraction _objectInteraction;
    private Rigidbody _rigidbody;
    private Rigidbody _armrigidbody;
    private SphereCollider _sphereCollider;
    private Score _score;

    private void Awake()
    {
        _arm = FindObjectOfType<ArmTag>();
        _rigidbody = GetComponent<Rigidbody>();
        _sphereCollider = GetComponentInChildren<SphereCollider>();
        _armrigidbody = _arm.GetComponent<Rigidbody>();
        _objectInteraction = new ObjectInteraction(_rigidbody, transform, _armrigidbody, _sphereCollider);
        _score = FindObjectOfType<Score>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG) && _arm.gameObject.transform.childCount == 0)
        {
            _objectInteraction.Drag();
            _objectInteraction.AddFixedJoint(transform, _armrigidbody);
            transform.SetParent(_arm.gameObject.transform);
        }
       
        if (other.CompareTag(BOX_TAG))
        {
            var fixedJoint = GetComponent<FixedJoint>();
            Destroy(fixedJoint);
            gameObject.transform.parent = null;
            _objectInteraction.Drop(_forceY, _forceZ);
        }
    }
    private async void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            _objectInteraction.EnableInteractionCollider();          
        }

        if (collision.gameObject.CompareTag(BOX_FLOOR_TAG))
        {
            _objectInteraction.DisableInteractionCollider();
            _score.ApplyScore(_scoreValue);
            TextScoreUI.Instance.AddText(_scoreValue.Value, transform.position);
            await UniTask.Delay(1000);
            Destroy(gameObject);
        }
    }
}

using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Item : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string BOX_TAG = "Box";
    private const string BOX_FLOOR_TAG = "BoxFloor";
    private const string GROUND_TAG = "Ground";

    [SerializeField] private int _scoreValue = 100;
    [SerializeField] private float _forceZ = 20f;
    [SerializeField] private float _forceY = 20f;

    private CompositeDisposable _disposable = new CompositeDisposable();
    private ArmTag _arm;
    private ObjectInteraction _objectInteraction;
    private Rigidbody _rigidbody;
    private Rigidbody _armrigidbody;
    private SphereCollider _sphereCollider;
    private Score _score;
    private BoxCollider _collider;

    private void Awake()
    {
        _arm = FindObjectOfType<ArmTag>();
        _rigidbody = GetComponent<Rigidbody>();
        _sphereCollider = GetComponentInChildren<SphereCollider>();
        _armrigidbody = _arm.GetComponent<Rigidbody>();
        _objectInteraction = new ObjectInteraction(_rigidbody, transform, _armrigidbody, _sphereCollider);
        _score = FindObjectOfType<Score>();
        _collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _collider.OnTriggerEnterAsObservable()
         .Where(x => x.gameObject.CompareTag(PLAYER_TAG) && _arm.gameObject.transform.childCount == 0)
         .Subscribe(_ =>
         {
           _objectInteraction.Drag();
           _objectInteraction.AddFixedJoint(transform, _armrigidbody);
           transform.SetParent(_arm.gameObject.transform);

         }).AddTo(_disposable);

        _collider.OnTriggerEnterAsObservable()
           .Where(x => x.gameObject.CompareTag(BOX_TAG))
           .Subscribe(_ =>
           {
               var fixedJoint = GetComponent<FixedJoint>();
               Destroy(fixedJoint);
               gameObject.transform.parent = null;
               _objectInteraction.Drop(_forceY, _forceZ);

           }).AddTo(_disposable);

        _collider.OnCollisionEnterAsObservable()
          .Where(x => x.gameObject.CompareTag(GROUND_TAG))
          .Subscribe(_ =>
          {
              _objectInteraction.EnableInteractionCollider();

          }).AddTo(_disposable);

        _collider.OnCollisionEnterAsObservable()
         .Where(x => x.gameObject.CompareTag(BOX_FLOOR_TAG))
         .Subscribe(_ =>
         {
             _objectInteraction.DisableInteractionCollider();
             _score.ApplyScore(_scoreValue);
             TextScoreUI.Instance.AddText(_scoreValue, transform.position);
             Destroy(gameObject, 2f);

         }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
        _disposable.Dispose();
    }
}

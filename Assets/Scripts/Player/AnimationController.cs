using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _animator;
    private int _runHash = Animator.StringToHash("IsRun");
    private int _idleHash = Animator.StringToHash("IsIdle");

    private void Awake() =>   
        _animator = GetComponent<Animator>();
    
    public void SetIdleAnimation()
    {
        _animator.SetBool(_idleHash, true);
        _animator.SetBool(_runHash, false);
    }

    public void SetWalkingAnimation()
    {
        _animator.SetBool(_runHash, true);
        _animator.SetBool(_idleHash, false);
    }
}


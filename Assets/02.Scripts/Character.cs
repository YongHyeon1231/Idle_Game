using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    Animator _animator;

    protected Animator animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            return _animator;
        }
        private set => _animator = value;
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected void AnimatorTriggerChange(int hash)
    {
        animator.SetTrigger(hash);
    }

    protected void AnimatorChange(int hash)
    {
        animator.SetBool(Define.Character.hashIdle, false);
        animator.SetBool(Define.Character.hashMove, false);

        animator.SetBool(hash, true);
    }
}

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    Animator _animator;

    public double HP;
    public double ATK;
    public float ATK_Speed;
    public bool isDead = false;

    protected float Attack_Range = 5.0f; // 공격하는 공격 범위
    protected float target_Range = 10.0f; // 추격하는 범위
    protected bool isAttack = false;
    protected Transform m_Target;

    [SerializeField] protected Transform m_ProjectilePos;

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

    protected void InitAttack() => isAttack = false;

    protected void AnimatorTriggerChange(int hash)
    {
        animator.SetTrigger(hash);
    }

    protected void AnimatorChange(int hash)
    {
        if (hash == Define.Character.hashATK01)
        {
            AnimatorTriggerChange(hash);
            return;
        }

        animator.SetBool(Define.Character.hashIdle, false);
        animator.SetBool(Define.Character.hashMove, false);

        animator.SetBool(hash, true);
    }

    protected virtual void ProjectileAttack()
    {
        if (m_Target == null) return;
        
        Base_Manager.Pool.Pooling_OBJ("Projectile").Get((value) =>
        {
            value.transform.position = m_ProjectilePos.position;
            value.GetComponent<Projectile>().Init(m_Target, ATK, "CH_01_01");
        });
    }

    protected void FindClosetTarget<T>(T[] targets) where T : Component
    {
        var monsters = targets;
        Transform closetTarget = null;
        float maxDistance = target_Range;

        foreach (var monster in monsters)
        {
            float targetDistance = Vector3.Distance(transform.position, monster.transform.position);

            if (targetDistance <= maxDistance)
            {
                closetTarget = monster.transform;
                maxDistance = targetDistance;
            }
        }
        m_Target = closetTarget;
        if(m_Target != null) 
        {
            transform.LookAt(m_Target.position);
        }
    }
}

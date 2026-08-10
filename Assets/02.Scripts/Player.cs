using UnityEngine;

public class Player : Character
{
    Vector3 startPos;
    Quaternion rot;

    protected override void Start()
    {
        base.Start();

        startPos = transform.position;
        rot = transform.rotation;
    }

    void Update()
    {
        FindClosetTarget(Spawner.m_Monsters.ToArray());
        
        if(m_Target == null)
        {
            float targetPos = Vector3.Distance(transform.position, startPos);
            if(targetPos > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
                transform.LookAt(startPos);
                AnimatorChange(Define.Character.hashMove);
            }
            else
            {
                transform.rotation = rot;
                AnimatorChange(Define.Character.hashIdle);
            }
            return;
        }

        if(m_Target.GetComponent<Character>().isDead)
        {
            FindClosetTarget(Spawner.m_Monsters.ToArray());
        }

        float targetDistance = Vector3.Distance(transform.position, m_Target.position);
        if (targetDistance <= target_Range && targetDistance > Attack_Range && isAttack == false) // 현재 타겟이 추적 범위안에는 있지만 공격범위 안에는 없을 때
        {
            AnimatorChange(Define.Character.hashMove);
            transform.LookAt(m_Target.position);
            transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);
        }
        else if (targetDistance <= Attack_Range && isAttack == false) // 현재 타겟이 공격 범위 안에 있을 때
        {
            isAttack = true;
            AnimatorTriggerChange(Define.Character.hashATK01);
            Invoke("InitAttack", 1.0f);
        }
    }
}

using System.Collections;
using UnityEngine;

public class Monster : Character
{
    public float m_Speed;

    bool isSpawn = false;

    protected override void Start()
    {
        base.Start();
    }

    public void Init()
    {
        isDead = false;
        HP = 5;
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (!isSpawn) return;

        transform.LookAt(Vector3.zero);

        float targetDistance = Vector3.Distance(transform.position, Vector3.zero);
        if (targetDistance <= 0.9f)
            AnimatorChange(Define.Character.hashIdle);
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * m_Speed);
            AnimatorChange(Define.Character.hashMove);
        }
    }

    IEnumerator SpawnRoutine()
    {
        float endScale = transform.localScale.x;
        transform.localScale = Vector3.zero;
        transform.LookAt(Vector3.zero);

        AnimatorTriggerChange(Define.Character.hashSpawn);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"));

        float current = 0f, percent = 0f;
        while (percent < 1f || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            current += Time.deltaTime;
            percent = Mathf.Clamp01(current / 1f);
            transform.localScale = Vector3.one * Mathf.Lerp(0f, endScale, percent);
            yield return null;
        }

        isSpawn = true;
    }

    public void GetDamage(double dmg)
    {
        if (isDead) return;

        Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
           value.GetComponent<HIT_TEXT>().Init(transform.position, dmg, false); 
        });

        HP -= dmg;
        if (HP <= 0)
        {
            isDead = true;
            Spawner.m_Monsters.Remove(this);

            var smokeObj = Base_Manager.Pool.Pooling_OBJ("Smoke").Get((value) =>
            {
                value.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                Base_Manager.instance.Return_Pool(value.GetComponent<ParticleSystem>().main.duration, value.gameObject, "Smoke");
            });

            Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }
    }
}

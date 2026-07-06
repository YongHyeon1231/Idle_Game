using System.Collections;
using UnityEngine;

public class Monster : Character
{
    public float m_Speed;

    bool isSpawn = true;

    public void Init()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (isSpawn) return;

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

        isSpawn = false;
    }

    
}

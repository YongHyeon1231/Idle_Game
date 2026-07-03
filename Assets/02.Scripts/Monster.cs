using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float m_Speed;
    Animator animator;
    bool isSpawn = true;

    static readonly int hashIdle  = Animator.StringToHash("isIDLE");
    static readonly int hashMove  = Animator.StringToHash("isMOVE");
    static readonly int hashSpawn = Animator.StringToHash("isSPAWN");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
            AnimatorChange(hashIdle);
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * m_Speed);
            AnimatorChange(hashMove);
        }
    }

    IEnumerator SpawnRoutine()
    {
        float endScale = transform.localScale.x;
        transform.localScale = Vector3.zero;
        transform.LookAt(Vector3.zero);

        animator.SetTrigger(hashSpawn);

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

    private void AnimatorChange(int hash)
    {
        animator.SetBool(hashIdle, false);
        animator.SetBool(hashMove, false);

        animator.SetBool(hash, true);
    }
}

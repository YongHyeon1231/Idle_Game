using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject monster_Prefab;

    public int m_Count; // 몬스터의 수
    public float m_SpawnTime; // 몇 초 마다

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        Vector3 pos;

        for(int i = 0; i < m_Count; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 10.0f;
            pos.y = 0.0f;

            while(Vector3.Distance(pos, Vector3.zero) <= 5.0f)
            {
                pos = Vector3.zero + Random.insideUnitSphere * 10.0f;
                pos.y = 0.0f;
            }

            var goObj = Base_Manager.Pool.Pooling_OBJ("Monster").Get((value) =>
            {
                value.GetComponent<Monster>().Init();
                value.transform.position = pos;
                value.transform.LookAt(Vector3.zero);
            });

            StartCoroutine(ReturnCoroutine(goObj));
        }

        yield return new WaitForSeconds(m_SpawnTime);
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator ReturnCoroutine(GameObject obj)
    {
        yield return new WaitForSeconds(3.0f);
        Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(obj);
    }
}

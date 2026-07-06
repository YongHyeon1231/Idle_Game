using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int m_Count; // 몬스터의 수
    public float m_SpawnTime; // 몇 초 마다

    public static List<Monster> m_Monsters = new List<Monster>();
    public static List<Player> m_Players = new List<Player>();

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
                m_Monsters.Add(value.GetComponent<Monster>());
            });
        }

        yield return new WaitForSeconds(m_SpawnTime);
        StartCoroutine(SpawnCoroutine());
    }
}

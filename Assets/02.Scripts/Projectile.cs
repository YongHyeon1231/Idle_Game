using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    Transform m_Target;
    Vector3 m_TargetPos;
    double m_DMG = 10.0f;
    string m_CharacterName;
    bool GetHit = false;

    Dictionary<string, GameObject> m_Projectiles = new Dictionary<string, GameObject>();
    Dictionary<string, ParticleSystem> m_Particles = new Dictionary<string, ParticleSystem>();

    private void Awake()
    {
        Transform projectiles = transform.GetChild(0);
        Transform particles = transform.GetChild(1);

        for(int i = 0; i < projectiles.childCount; i++)
        {
            m_Projectiles.Add(projectiles.GetChild(i).name, projectiles.GetChild(i).gameObject);
        }

        for(int i = 0; i < particles.childCount; i++)
        {
            m_Particles.Add(particles.GetChild(i).name, particles.GetChild(i).GetComponent<ParticleSystem>());
        }
    }
    
    public void Init(Transform target, double dmg, string Character_Name)
    {
        m_Target = target;
        transform.LookAt(m_Target);
        GetHit = false;

        m_TargetPos = m_Target.position;

        m_DMG = dmg;

        m_CharacterName = Character_Name;
        m_Projectiles[m_CharacterName].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (GetHit) return;
        
        m_TargetPos.y = 0.5f;

        transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, Time.deltaTime * m_Speed);
        if (Vector3.Distance(transform.position, m_TargetPos) <= 0.1f)
        {
            if(m_Target != null)
            {
                GetHit = true;

                m_Target.GetComponent<Monster>().GetDamage(m_DMG);

                m_Projectiles[m_CharacterName].gameObject.SetActive(false);
                m_Particles[m_CharacterName].Play();

                StartCoroutine(ReturnObject(m_Particles[m_CharacterName].main.duration));
            }
        }
    }

    IEnumerator ReturnObject(float timer)
    {
        yield return new WaitForSeconds(timer);
        Base_Manager.Pool.m_pool_Dictionary["Projectile"].Return(this.gameObject);
    }
}

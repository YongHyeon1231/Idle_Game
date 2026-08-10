using System.Collections;
using TMPro;
using UnityEngine;

public class Item_OBJ : MonoBehaviour
{
    [SerializeField] private Transform ItemTextRect;
    [SerializeField] private TextMeshProUGUI m_Text;
    [SerializeField] private GameObject[] Raritys;
    [SerializeField] private ParticleSystem m_Loot;

    [SerializeField] private float firingAngle = 45.0f;
    [SerializeField] private float gravity = 9.8f;

    Rarity rarity;

    bool isCheck = false;

    void RarityCheck()
    {
        isCheck = true;

        transform.rotation = Quaternion.identity; // (0,0,0)

        Raritys[(int)rarity].SetActive(true);

        ItemTextRect.gameObject.SetActive(true);
        ItemTextRect.SetParent(Base_Canvas.instance.HOLDER_LAYER(2));

        m_Text.text = Utils.String_Color_Rarity(rarity) + "TEST_ITEM" + "</color>";
        // <color=#FFFFFF>TEST ITEM</color>
        // <size=35>TEST ITEM</size>

        StartCoroutine(LootItem());
    }

    IEnumerator LootItem()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

        for (int i = 0; i < Raritys.Length; i++)
        {
            Raritys[i].SetActive(false);
        }

        // ItemTextRect.transform.parent = this.transform;
        ItemTextRect.SetParent(this.gameObject.transform);
        ItemTextRect.gameObject.SetActive(false);

        m_Loot.Play();

        yield return new WaitForSeconds(0.5f);

        Base_Manager.Pool.m_pool_Dictionary["Item_OBJ"].Return(this.gameObject);
    }

    private void Update()
    {
        if (isCheck == false) return;

        ItemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Init(Vector3 pos) // pos는 몬스터 위치
    {
        rarity = (Rarity)Random.Range(0, 5); // 임시로 아이템 랜덤 등급 획득

        isCheck = false;
        transform.position = pos;

        // 풀링으로 재사용될 때 이전 위치의 파티클/트레일 잔상이 새 위치까지 이어져 보이는 것을 방지
        foreach (var ps in GetComponentsInChildren<ParticleSystem>(true))
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
        }

        Vector3 Target_Pos = new Vector3(pos.x + (Random.insideUnitSphere.x * 2.0f), 0.5f, pos.z + (Random.insideUnitSphere.z * 2.0f));
        StartCoroutine(SimulateProjectile(Target_Pos)); // pos 위치로 곡선을 그리며 이동해라.
    }

    // 포물선(투사체) 운동을 시뮬레이션해서 오브젝트를 목표 지점(pos)까지 곡선으로 이동시키는 코루틴
    IEnumerator SimulateProjectile(Vector3 pos)
    {
        float target_Distance = Vector3.Distance(transform.position, pos);
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        float flightDuration = target_Distance / Vx;

        transform.rotation = Quaternion.LookRotation(pos - transform.position); // 현재 오브젝트가 바라보는 방향으로 이동

        float time = 0.0f;
        while (time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * time)) * Time.deltaTime, Vx * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        RarityCheck();
    }
}

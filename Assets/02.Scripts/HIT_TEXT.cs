using TMPro;
using UnityEngine;

public class HIT_TEXT : MonoBehaviour
{
    Vector3 target;
    Camera cam;
    public TextMeshProUGUI m_Text;

    [SerializeField] private GameObject m_Critical;
    private float UpRange = 0.0f;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Init(Vector3 pos, double dmg, bool Critical = false)
    {
        pos.x += Random.Range(-0.5f, 0.5f);
        pos.z += Random.Range(-0.5f, 0.5f);

        target = pos;
        m_Text.text = dmg.ToString();
        // transform.parent = Base_Canvas.instance.transform;
        // transform.SetParent(Base_Canvas.instance.transform, false);
        transform.SetParent(Base_Canvas.instance.HOLDER_LAYER(1));

        m_Critical.SetActive(Critical);

        Base_Manager.instance.Return_Pool(2.0f, this.gameObject, "HIT_TEXT");
    }

    private void Update()
    {
        Vector3 targetPos = new Vector3(target.x, target.y + UpRange, target.z);
        transform.position = cam.WorldToScreenPoint(targetPos);
        if (UpRange <= 0.3f)
        {
            UpRange += Time.deltaTime;
        }
    }

    private void ReturnText()
    {
        Base_Manager.Pool.m_pool_Dictionary["HIT_TEXT"].Return(this.gameObject);
    }
}

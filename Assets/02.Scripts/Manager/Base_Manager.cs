using System.Collections;
using UnityEngine;

public class Base_Manager : MonoBehaviour
{
    public static Base_Manager instance = null;

    private static Pool_Manager s_Pool = new Pool_Manager();
    public static Pool_Manager Pool { get { return s_Pool; } }

    private void Awake()
    {
        Initalize();
    }

    private void Initalize()
    {
        if(instance == null)
        {
            instance = this;
            Pool.Initialize(transform);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject Instantiate_Path(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }

    public void Return_Pool(float timer, GameObject obj, string path)
    {
        StartCoroutine(Return_Pool_Coroutine(timer, obj, path));
    }

    IEnumerator Return_Pool_Coroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        Pool.m_pool_Dictionary[path].Return(obj);
    }
}

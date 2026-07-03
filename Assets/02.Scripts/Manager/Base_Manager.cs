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
}

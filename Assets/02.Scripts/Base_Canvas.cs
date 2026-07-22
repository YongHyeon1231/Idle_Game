using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Transform COIN;
    [SerializeField] private Transform LAYER;

    public Transform HOLDER_LAYER(int value)
    {
        return LAYER.GetChild(value);
    }
}

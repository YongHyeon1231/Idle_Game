using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    Transform parentTransform {get; set;}
    Queue<GameObject> pool {get; set;}
    GameObject Get(Action<GameObject> action = null);
    void Return(GameObject obj, Action<GameObject> action = null);
}

public class Object_Pool : IPool
{
    // Dequeue : 큐에서 가장 앞에 있는 요소를 제거하고 반환
    // Enqueue : 큐의 가장 뒤에 요소를 추가
    public Queue<GameObject> pool {get; set;} = new Queue<GameObject>();

    public Transform parentTransform {get; set;}

    // Get : 큐에서 가장 앞에 있는 요소를 제거하고 반환
    public GameObject Get(Action<GameObject> action = null)
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        if (action != null)
        {
            action?.Invoke(obj);
        }
        return obj;
    }

    // Return : 큐에 요소를 추가
    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj);
        // obj.transform.parent = parentTransform;
        obj.transform.SetParent(parentTransform, false);
        obj.SetActive(false);
        if (action != null)
        {
            action?.Invoke(obj);
        }
    }
}

public class Pool_Manager
{
    public Dictionary<string, IPool> m_pool_Dictionary = new Dictionary<string, IPool>();

    Transform base_Obj = null;

    public void Initialize(Transform T)
    {
        base_Obj = T;
    }

    public IPool Pooling_OBJ(string path)
    {
        if(m_pool_Dictionary.ContainsKey(path) == false)
        {
            Add_Pool(path);
        }

        if(m_pool_Dictionary[path].pool.Count <= 0)
        {
            Add_Queue(path);
        }

        return m_pool_Dictionary[path];
    }

    private GameObject Add_Pool(string path)
    {
        GameObject obj = new GameObject(path + "##POOL");
        obj.transform.SetParent(base_Obj);
        Object_Pool T_Component = new Object_Pool();

        m_pool_Dictionary.Add(path, T_Component);

        T_Component.parentTransform = obj.transform;
        return obj;
    }

    private void Add_Queue(string path)
    {
        // var go = Instantiate(Resources.Load<GameObject>(path)); // Pool_Manager에서 Instantiate를 사용하면, Pool_Manager가 Resources에 의존하게 된다. 따라서, Pool_Manager는 Resources에 의존하지 않도록 수정해야 한다.
        var go = Base_Manager.instance.Instantiate_Path(path);
        // go.transform.parent = m_pool_Dictionary[path].parentTransform;
        go.transform.SetParent(m_pool_Dictionary[path].parentTransform, false);

        m_pool_Dictionary[path].Return(go);
    }
}

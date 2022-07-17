using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PoolSystem : MonoBehaviour
{
    public static PoolSystem Instance { get; private set; }

    [SerializeField] private bool hideWarnings;
    [SerializeField] private int nextId;
    [SerializeField] private int nextGlobalPoolId;

    private Dictionary<string, Pool> namePoolPairs = new();

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        Instance = this;
    }

    public void Enpool(GameObject go)
    {
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable == null)
        {
            if (!hideWarnings) Debug.LogWarning("Enpooled " + go + ", but it had no Poolable component. Using Destroy().");
            Destroy(go);
        } else
        {
            string name = poolable.name;
            if (!namePoolPairs.ContainsKey(name))
            {
                if (!hideWarnings) Debug.LogWarning("Enpooled " + go + ", but it had an invalid pool name. Using Destroy().");
                Destroy(go);
            } else
            {
                Pool pool = namePoolPairs[name];
                pool.pooled.Enqueue(go);
                go.SetActive(false);
            }
        }
    }
    public GameObject Depool(GameObject prefab)
    {
        Pool pool = null;
        string name = prefab.name;
        
        if (namePoolPairs.ContainsKey(name))
            pool = namePoolPairs[name];
        
        if (pool == null)
        {
            pool = new(prefab, name);
            namePoolPairs.Add(name, pool);
        }

        GameObject go;
        Poolable poolable;
        bool instantiate = pool.pooled.Count == 0;
        if (instantiate)
        {
            go = Instantiate(prefab);
            poolable = go.AddComponent<Poolable>();
            poolable.LocalPoolId = pool.nextLocalId;
            poolable.GlobalPoolId = nextGlobalPoolId;
            poolable.name = name;
            pool.nextLocalId++;
            nextGlobalPoolId++;
        } else
        {
            go = pool.pooled.Dequeue();
            poolable = go.GetComponent<Poolable>();
        }

        go.SetActive(prefab.activeSelf);
        poolable.Id = nextId;
        nextId++;
        return go;
    }

    private class Pool
    {
        public string name;
        public int nextLocalId;
        public GameObject prefab;
        public Queue<GameObject> pooled;

        public Pool(GameObject prefab, string name)
        {
            this.name = name;
            this.prefab = prefab;
            pooled = new();
        }
    }
}

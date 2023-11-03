using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class Pool_BallRun2048
{
    public string name;
    public GameObject go;
    public int count;
    public List<GameObject> actives;
    public Queue<GameObject> deactives;

    public Pool_BallRun2048(string name, GameObject go, int count)
    {
        this.name = name;
        this.go = go;
        this.count = count;
    }

    public void InitaPool(Transform container, Dictionary<int, int> dicClones)
    {
        actives = new List<GameObject>();
        deactives = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            spawnAClone(container, dicClones);
        }
    }

    void spawnAClone(Transform container, Dictionary<int, int> dicClones)
    {
        var clone = Object.Instantiate(go, container);
        clone.transform.localScale = Vector3.one;
        clone.name += (actives.Count + deactives.Count);
        deactives.Enqueue(clone);
        dicClones.Add(clone.GetHashCode(), GetHashCode());
    }

    public GameObject Get(Transform container, Dictionary<int, int> dicClones)
    {
        if (deactives.Count == 0)
            spawnAClone(container, dicClones);
        var clone = deactives.Dequeue();
        actives.Add(clone);
        return clone;
    }

    public void Return(GameObject go)
    {
        go.SetActive(false);
        actives.Remove(go);
        deactives.Enqueue(go);
    }

    public void OnDestroy()
    {
        foreach (var active in actives)
        {
            active.transform.DOKill();
        }

        foreach (var deactive in deactives)
        {
            deactive.transform.DOKill();
        }
    }
}



public class ObjectPool_BallRun2048 : MonoBehaviour
{
    public static ObjectPool_BallRun2048 instance;

    public Pool_BallRun2048[] balls;
    public Pool_BallRun2048[] powerUps;

    public Dictionary<int, int> dicClones = new Dictionary<int, int>();
    public List<Pool_BallRun2048> pools = new List<Pool_BallRun2048>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        foreach (Pool_BallRun2048 ball in balls)
        {
            pools.Add(ball);
        }

        foreach (Pool_BallRun2048 powerUp in powerUps)
        {
            pools.Add(powerUp);
        }

        foreach (var pool in pools)
        {
            pool.InitaPool(transform, dicClones);
        }
    }

    public Pool_BallRun2048 TryAddPoolByScript(Pool_BallRun2048 p)
    {
        var existedPool = pools.Find(x => x.go == p.go);
        if (existedPool != null)
        {
            Debug.LogWarning($"existed pool: {p.go.name}", p.go.transform);
            return existedPool;
        }
        pools.Add(p);
        p.InitaPool(transform, dicClones);
        return p;
    }

#if UNITY_EDITOR
    private void Update()
    {
        foreach (var p in pools)
        {
            var activeCount = p.actives.Count;
            var deactiveCount = p.deactives.Count;
            p.name = $"total: {activeCount + deactiveCount} | active: {activeCount} | deactive: {deactiveCount}";
        }
    }
#endif

    public GameObject Get(Pool_BallRun2048 p)
    {
        return p.Get(transform, dicClones);
    }
    public GameObject GetFromObjectPool(Pool_BallRun2048 p, Vector3 transformPos)
    {
        GameObject newEnemy = p.Get(transform, dicClones);
        newEnemy.SetActive(true);
        newEnemy.transform.position = transformPos;

        return newEnemy;
    }


    public void Return(GameObject clone)
    {
        clone.SetActive(false);
        clone.transform.DOKill();
        var hash = clone.GetHashCode();
        if (dicClones.ContainsKey(hash))
        {
            var p = getPool(dicClones[hash]);
            p.Return(clone);
        }
    }
    Pool_BallRun2048 getPool(int hash)
    {
        foreach (var pool in pools)
        {
            if (pool.GetHashCode() == hash)
                return pool;
        }

        return null;
    }

    private void OnDestroy()
    {
        foreach (var pool in pools)
        {
            pool.OnDestroy();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> 
    where T : Component, IPoolable
{
    private readonly Queue<T> _pool = new Queue<T>();
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly int _initialSize;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
        _initialSize = initialSize;
        Initialize();
    }

    private void Initialize()
    {
        for (var i = 0; i < _initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject()
    {
        var newObject = Object.Instantiate(_prefab, _parent);
        newObject.gameObject.SetActive(false);
        newObject.transform.SetParent(_parent);
        _pool.Enqueue(newObject);
        return newObject;
    }

    public T Spawn(Vector3 position, Quaternion quaternion, Vector3? size = null)
    {
        var obj = _pool.Count > 0 ? _pool.Dequeue() : CreateNewObject();
        obj.transform.SetPositionAndRotation(position, quaternion);
        if (size.HasValue)
        {
            obj.transform.localScale = size.Value;
        }
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
        return obj;
    }
    
    public void Despawn(T obj)
    {
        obj.OnDespawn();
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
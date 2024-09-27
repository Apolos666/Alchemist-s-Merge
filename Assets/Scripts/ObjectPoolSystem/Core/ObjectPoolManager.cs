using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : GenericSingleton<ObjectPoolManager>
{
    private readonly Dictionary<Type, object> _pools = new Dictionary<Type, object>();
    
    public void CreatePool<T>(T prefab, Transform parent, int initialSize) 
        where T : Component, IPoolable
    {
        var type = typeof(T);
        if (!_pools.ContainsKey(type))
        {
            _pools[type] = new ObjectPool<T>(prefab, initialSize, parent);
        }
    }
    
    public T Spawn<T>(Vector3 position, Quaternion quaternion, Vector3? size = null) 
        where T : Component, IPoolable
    {
        var type = typeof(T);
        if (_pools.TryGetValue(type, out var pool))
        {
            return ((ObjectPool<T>) pool).Spawn(position, quaternion, size);
        }

        throw new Exception($"Pool of type {type.Name} doesn't exist.");
    }
    
    public void Despawn<T>(T obj) where T : Component, IPoolable
    {
        var type = typeof(T);
        if (_pools.TryGetValue(type, out var pool))
        {
            ((ObjectPool<T>) pool).Despawn(obj);
        }
        else
        {
            throw new Exception($"Pool of type {type.Name} doesn't exist.");
        }
    }
}
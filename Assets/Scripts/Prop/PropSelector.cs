using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropSelector : GenericSingleton<PropSelector>
{
    [SerializeField] private Prop[] _props;

    private readonly Queue<Prop> _propQueue = new Queue<Prop>();
    private const int QueueSize = 2;

    public Prop CurrentProp => _propQueue.Count > 0 ? _propQueue.Peek() : null;

    private void Start()
    {
        InitializePropQueue();
    }

    private void InitializePropQueue()
    {
        for (var i = 0; i < QueueSize; i++)
        {
            AddNewPropToQueue();
        }
    }

    private void AddNewPropToQueue()
    {
        var randomProp = _props[Random.Range(0, _props.Length)];
        EventBus.Publish(new AddNewPropToQueue(randomProp));
        _propQueue.Enqueue(randomProp);
    }

    public (GameObject, Prop) GetNextProp(Vector3 spawnPoint)
    {
        var prop = _propQueue.Dequeue();
        var newProp = Instantiate(prop.Prefab, spawnPoint, Quaternion.identity);
        
        if (_propQueue.Count > 0)
            EventBus.Publish(new NextPropReadyEvent(_propQueue.Peek()));
        
        AddNewPropToQueue();
        return (newProp, prop);
    }
}
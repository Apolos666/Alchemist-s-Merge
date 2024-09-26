using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropSelector : GenericSingleton<PropSelector>
{
    [SerializeField] private Prop[] _props;

    private readonly Queue<Prop> _propQueue = new Queue<Prop>();
    private const int QueueSize = 2;

    public Prop CurrentProp
    {
        get
        {
            if (_propQueue != null && _propQueue.Count > 0)
                _propQueue.Peek();
            
            return null;
        }
    }

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
        _propQueue.Enqueue(randomProp);
    }

    public GameObject GetNextProp(Vector3 spawnPoint)
    {
        var prop = _propQueue.Dequeue();
        var newProp = Instantiate(prop.Prefab, spawnPoint, Quaternion.identity);
        newProp.GetComponent<Rigidbody2D>().gravityScale = 0;
        AddNewPropToQueue();
        return newProp;
    }
}

public class ScoreChangedEvent : IEvent
{
    public int Score { get; }

    public ScoreChangedEvent(int score)
    {
        Score = score;
    }
}
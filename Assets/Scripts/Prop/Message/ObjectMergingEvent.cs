using UnityEngine;

public class ObjectMergingEvent : IEvent
{
    public Vector3 MergePosition { get; }
    public Vector3 NextPropSize { get; }
    public int Point { get; }
    public ObjectMergingEvent(Vector3 mergePosition, Vector3 nextPropSize, int point)
    {
        NextPropSize = nextPropSize;
        MergePosition = mergePosition;
        Point = point;
    }
}
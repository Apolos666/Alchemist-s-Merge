using UnityEngine;

public class PropBeingDestroyEvent : IEvent
{
    public Vector3 MergePosition { get; }    
    public Vector3 NextPropSize { get; }
    public int Point { get; }
    
    public PropBeingDestroyEvent(Vector3 mergePosition, int point, Vector3 nextPropSize)
    {
        MergePosition = mergePosition;
        Point = point;
        NextPropSize = nextPropSize;
    }
}
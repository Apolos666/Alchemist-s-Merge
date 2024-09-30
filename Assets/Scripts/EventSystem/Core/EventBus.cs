using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, List<WeakSubscription>> Subscribers = new Dictionary<Type, List<WeakSubscription>>();

    private class WeakSubscription
    {
        private readonly WeakReference _targetRef;
        private readonly Delegate _handler;

        public WeakSubscription(object target, Delegate handler)
        {
            _targetRef = new WeakReference(target);
            _handler = handler;
        }

        public bool IsAlive => _targetRef.IsAlive;

        public void Invoke(object[] args)
        {
            if (_targetRef.Target != null)
            {
                _handler.DynamicInvoke(args);
            }
        }

        public bool Matches(object target, Delegate handler)
        {
            return _targetRef.Target == target && _handler == handler;
        }
    }

    public static void Subscribe<T>(Action<T> handler) where T : IEvent
    {
        var eventType = typeof(T);

        if (!Subscribers.TryGetValue(eventType, out var subscriptions))
        {
            subscriptions = new List<WeakSubscription>();
            Subscribers[eventType] = subscriptions;
        }

        var target = handler.Target ?? typeof(EventBus);
        subscriptions.Add(new WeakSubscription(target, handler));
    }

    public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
    {
        var eventType = typeof(T);

        if (Subscribers.TryGetValue(eventType, out var subscriptions))
        {
            var target = handler.Target ?? typeof(EventBus);
            subscriptions.RemoveAll(s => s.Matches(target, handler));
        }
    }

    public static void Publish<T>(T eventData) where T : IEvent
    {
        var eventType = typeof(T);
        if (Subscribers.TryGetValue(eventType, out var subscriptions))
        {
            foreach (var subscription in subscriptions.ToList().Where(subscription => subscription.IsAlive))
            {
                try
                {
                    subscription.Invoke(new object[] { eventData });
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error publishing event of type {eventType}: {e}");
                }
            }

            // Clean up dead subscriptions
            subscriptions.RemoveAll(s => !s.IsAlive);
        }
    }

    public static void ClearAllSubscribers()
    {
        Subscribers.Clear();
        Debug.Log("Cleared all subscribers");
    }

    public static void ClearSubscribers<T>() where T : IEvent
    {
        var eventType = typeof(T);
        if (Subscribers.TryGetValue(eventType, out var subscriber))
        {
            subscriber.Clear();
            Debug.Log($"Cleared subscribers for {eventType}");
        }
    }
}
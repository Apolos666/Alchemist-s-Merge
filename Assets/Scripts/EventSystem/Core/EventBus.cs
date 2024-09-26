using System;
using System.Collections.Generic;

public static class EventBus
{
    // Dictionary lưu trữ các danh sách các hành động (subscribers) cho từng loại sự kiện
    private static readonly Dictionary<Type, List<Action<IEvent>>> Subscribers = new Dictionary<Type, List<Action<IEvent>>>();
    
    /// <summary>
    /// Đăng ký một handler cho một loại sự kiện cụ thể
    /// </summary>  
    /// <param name="handler">Hàm xử lý sự kiện</param>
    /// <typeparam name="T">Loại sự kiện</typeparam>
    public static void Subscribe<T>(Action<T> handler) 
        where T : IEvent
    {
        var eventType = typeof(T);
        
        if (!Subscribers.ContainsKey(eventType))
        {
            Subscribers[eventType] = new List<Action<IEvent>>();
        }
        
        Subscribers[eventType].Add(evt => handler((T) evt));
    }
    
    /// <summary>
    /// Hủy đăng ký một handler cho một loại sự kiện cụ thể
    /// </summary>
    /// <param name="handler">Hàm xử lý sự kiện</param>
    /// <typeparam name="T">Loại sự kiện</typeparam>
    public static void Unsubscribe<T>(Action<T> handler) 
        where T : IEvent
    {
        var eventType = typeof(T);

        if (Subscribers.TryGetValue(eventType, out var handlers))
        {
            handlers.RemoveAll(h => h.Target == handler.Target && h.Method == handler.Method);
        }
    }
    
    /// <summary>
    /// Phát hành một sự kiện đến tất cả các subscribers đã đăng ký
    /// </summary>
    /// <param name="eventData">Dữ liệu sự kiện</param>
    /// <typeparam name="T">Loại sự kiện</typeparam>
    public static void Publish<T>(T eventData) 
        where T : IEvent
    {
        var eventType = typeof(T);
        if (Subscribers.TryGetValue(eventType, out var subscriber))
        {
            foreach (var handler in subscriber)
            {
                try
                {
                    handler(eventData);
                } catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
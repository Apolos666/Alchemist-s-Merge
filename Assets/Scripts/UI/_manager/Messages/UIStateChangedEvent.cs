public class UIStateChangedEvent : IEvent
{
    public bool IsOverlayActive { get; }
    
    public UIStateChangedEvent(bool isOverlayActive) => IsOverlayActive = isOverlayActive;
}
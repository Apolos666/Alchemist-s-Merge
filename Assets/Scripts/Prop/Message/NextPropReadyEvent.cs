public class NextPropReadyEvent : IEvent
{
    public Prop Prop { get; }
    public NextPropReadyEvent(Prop prop) => Prop = prop;
}
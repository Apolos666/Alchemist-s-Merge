public class AddNewPropToQueue : IEvent
{
    public Prop Prop { get; }
    
    public AddNewPropToQueue(Prop prop) => Prop = prop;
}
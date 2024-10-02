public class EnchantedSeedAppliedEvent : IEvent
{
    public EnchantedProp EnchantedProp { get; }
    
    public EnchantedSeedAppliedEvent(EnchantedProp enchantedProp)
    {
        EnchantedProp = enchantedProp;
    }
}
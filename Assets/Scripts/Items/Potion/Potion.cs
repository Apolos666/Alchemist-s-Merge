public class Potion : GameItem
{
    private PotionItemData PotionData => itemData as PotionItemData;

    public float Strength => PotionData.Strength;
    public float EndValue => PotionData.EndValue;
    public float Duration => PotionData.Duration;
    
    public override void Use()
    {
        base.Use();
        if (CanUse())
        {
            OnItemUsed();
            EventBus.Publish(new PotionAppliedEvent());
            SaveQuantity();
        }
    }
}
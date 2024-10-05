using UnityEngine;

public class EnchantedSeed : GameItem
{
    [SerializeField] private EnchantedProp _enchantedProp;
    
    public override void Use()
    {
        base.Use();
        if (CanUse())
        {
            OnItemUsed();
            PropSelector.Instance.ApplySpecialSeed(_enchantedProp);
            EventBus.Publish(new EnchantedSeedAppliedEvent(_enchantedProp));
            SaveQuantity();
        }
    }
}
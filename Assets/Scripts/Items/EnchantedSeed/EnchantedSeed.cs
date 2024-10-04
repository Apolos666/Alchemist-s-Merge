using UnityEngine;

public class EnchantedSeed : GameItem
{
    [SerializeField] private EnchantedProp _enchantedProp;
    
    public override void Use()
    {
        if (CanUse())
        {
            OnItemUsed();
            PropSelector.Instance.ApplyEnchantedSeed(_enchantedProp);
            EventBus.Publish(new EnchantedSeedAppliedEvent(_enchantedProp));
            SaveQuantity();
        }
    }
}
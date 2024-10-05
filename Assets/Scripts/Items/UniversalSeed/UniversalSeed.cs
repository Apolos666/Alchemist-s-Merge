using UnityEngine;

public class UniversalSeed : GameItem
{
    [SerializeField] private UniversalSeedProp _universalSeedProp;
    
    public override void Use()
    {
        base.Use();
        if (CanUse())
        {
            OnItemUsed();
            PropSelector.Instance.ApplySpecialSeed(_universalSeedProp);
            EventBus.Publish(new UniversalSeedAppliedEvent(_universalSeedProp));
            SaveQuantity();
        }
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "NewPotionItemData", menuName = "Game/Potion Item Data")]
public class PotionItemData : GameItemData
{
    [SerializeField] private float strength;
    [SerializeField] private float endValue;
    [SerializeField] private float duration;

    public float Strength => strength;
    public float EndValue => endValue;
    public float Duration => duration;
}
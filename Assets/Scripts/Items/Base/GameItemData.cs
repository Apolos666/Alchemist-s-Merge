using UnityEngine;

[CreateAssetMenu(fileName = "NewGameItemData", menuName = "Game/Game Item Data")]
public class GameItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private int price;
    [SerializeField] private float cooldownTime;
    [SerializeField] private string itemId;
    [SerializeField] protected int _intialQuantity;

    public string ItemName => itemName;
    public int Price => price;
    public float CooldownTime => cooldownTime;
    public string ItemId => itemId;
    public int InitialQuantity => _intialQuantity;
}
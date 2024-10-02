using UnityEngine;
using UnityEngine.Serialization;

public abstract class GameItem : MonoBehaviour
{
    [SerializeField] protected string _itemName;
    [SerializeField] protected int _quantity;
    public int Quantity => _quantity; 
    [SerializeField] protected float _cooldownTime;

    private float _currentCooldownTime;

    protected virtual void Update()
    {
        if (_currentCooldownTime > 0) 
            _currentCooldownTime = Mathf.Max(0, _currentCooldownTime - Time.deltaTime);
    }
    
    public float GetCooldownProgress() => _currentCooldownTime / _cooldownTime;

    protected virtual bool CanUse() => _currentCooldownTime <= 0 && _quantity > 0;
    
    public abstract void Use();

    protected virtual void OnItemUsed()
    {
        _quantity--;
        _currentCooldownTime = _cooldownTime;
    }
}
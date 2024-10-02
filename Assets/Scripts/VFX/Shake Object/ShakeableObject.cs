using DG.Tweening;
using UnityEngine;

public class ShakeableObject : MonoBehaviour
{
    private Sequence _sequence;

    private void Start()
    {
        EventBus.Subscribe<ShakeEvent>(OnShake);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ShakeEvent>(OnShake);
        if (_sequence != null && _sequence.IsActive())
        {
            _sequence.Kill();
        }
    }

    private void OnShake(ShakeEvent message)
    {
        if (_sequence != null && _sequence.IsActive())
        {
            _sequence.Kill();
        }
        
        if (gameObject.layer == LayerMask.NameToLayer("Default")) return;
        
        _sequence = transform
            .DOJump(transform.position + Vector3.up * message.EndValue, message.Strength, 1, message.Duration)
            .SetEase(Ease.OutQuad);
    }
}
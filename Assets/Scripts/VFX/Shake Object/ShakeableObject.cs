using UnityEngine;

public class ShakeableObject : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        EventBus.Subscribe<ShakeEvent>(OnShake);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ShakeEvent>(OnShake);
    }

    private void OnShake(ShakeEvent message)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Default")) return;

        var force = Vector2.up * message.Strength;
        _rb.AddForce(force, ForceMode2D.Impulse);
    }
}

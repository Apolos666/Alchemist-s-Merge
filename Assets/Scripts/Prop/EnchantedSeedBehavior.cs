using System;
using System.Linq;
using UnityEngine;

public class EnchantedSeedBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _collisionVfx;
    [SerializeField] private float _checkRadius = 2f;
    [SerializeField] private LayerMask _checkLayers;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Prop"))
        {
            var collisionPoint = collision.GetContact(0).point;
            Instantiate(_collisionVfx, collisionPoint, Quaternion.identity);
            SoundEffectManager.Instance.PlaySound("Enchanted Prop Collision", Vector3.zero);
            
            Physics2D.CircleCastAll(collisionPoint, _checkRadius, Vector2.zero, 0f, _checkLayers)
                .Select(hit => hit.collider.GetComponent<PropBehavior>())
                .Where(prop => prop != null)
                .ToList()
                .ForEach(pb => pb.DestroyAndNotify());
            
            Destroy(gameObject);
        }
    }
}
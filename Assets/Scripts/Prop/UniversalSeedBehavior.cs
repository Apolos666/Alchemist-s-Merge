using System.Collections;
using UnityEngine;

public class UniversalSeedBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _collisionVfx;
    [SerializeField] private float _mergeDelay = 0.5f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Prop"))
        {
            var collisionPoint = other.GetContact(0).point;
            Instantiate(_collisionVfx, collisionPoint, Quaternion.identity);
            StartCoroutine(SpawnNextLevelProp(other.gameObject, collisionPoint));
        }
    }
    
    private IEnumerator SpawnNextLevelProp(GameObject otherProp, Vector2 midPoint)
    {
        // Vô hiệu hóa va chạm để ngăn chặn các va chạm khác trong quá trình xử lý
        GetComponent<Collider2D>().enabled = false;
        otherProp.GetComponent<Collider2D>().enabled = false;

        // Tạo hiệu ứng "hút" hai prop lại gần nhau
        var elapsedTime = 0f;
        var startPos = transform.position;
        var otherStartPos = otherProp.transform.position;

        while (elapsedTime < _mergeDelay)
        {
            var t = elapsedTime / _mergeDelay;
            transform.position = Vector3.Lerp(startPos, midPoint, t);
            otherProp.transform.position = Vector3.Lerp(otherStartPos, midPoint, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        var propBehavior = otherProp.GetComponent<PropBehavior>();
        
        if (propBehavior.NextLevel?.Prefab != null)
        {
            var nextLevelProp = Instantiate(propBehavior.NextLevel.Prefab, midPoint, Quaternion.identity);
            nextLevelProp.layer = LayerMask.NameToLayer("Item");
            SoundEffectManager.Instance.PlaySound("Universal Prop Collision", Vector3.zero);
        }
        
        propBehavior.DestroyAndNotify();
        Destroy(gameObject);
    }
}
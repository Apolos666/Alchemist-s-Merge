using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropBehavior : MonoBehaviour
{
    [SerializeField] private Prop _prop;
    [SerializeField] private Prop _nextLevel;
    public Prop NextLevel => _nextLevel;
    [SerializeField] private float _mergeDelay = 0.1f;
    [SerializeField] private float _spawnForce = 0.5f;

    private bool _isProcessing = false;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_isProcessing) return;
        
        var otherProp = other.gameObject.GetComponent<PropBehavior>();
        if (otherProp != null && otherProp._prop == _prop && !otherProp._isProcessing)
        {
            _isProcessing = true;
            otherProp._isProcessing = true;
            StartCoroutine(MergeProps(other.gameObject));
        }   
    }

    private IEnumerator MergeProps(GameObject otherProp)
    {
        // Vô hiệu hóa va chạm để ngăn chặn các va chạm khác trong quá trình xử lý
        GetComponent<Collider2D>().enabled = false;
        otherProp.GetComponent<Collider2D>().enabled = false;

        // Tạo hiệu ứng "hút" hai prop lại gần nhau
        var midPoint = (transform.position + otherProp.transform.position) / 2f;
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
        
        EventBus.Publish(
            new ObjectMergingEvent(
                midPoint,
                _nextLevel ? _nextLevel.Prefab.transform.localScale : Vector3.zero, 
                _prop.Point));
        
        TryEarnGold(midPoint);
        
        SpawnNextLevelProp(midPoint);
        
        PropManager.Instance.UnregisterProp(otherProp);
        PropManager.Instance.UnregisterProp(gameObject);
        Destroy(otherProp);
        Destroy(gameObject);
    }

    private void SpawnNextLevelProp(Vector3 spawnPosition)
    {
        if (_nextLevel?.Prefab is null) return;  
        
        var newProp = Instantiate(_nextLevel.Prefab, spawnPosition, Quaternion.identity);
        newProp.layer = LayerMask.NameToLayer("Item");
        PropManager.Instance.RegisterProp(newProp);
        
        // Thêm một lực nhỏ theo hướng ngẫu nhiên để tạo hiệu ứng "bật" ra
        var rb = newProp.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            var randomDirection = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDirection * _spawnForce, ForceMode2D.Impulse);
        }
    }
    
    public void DestroyAndNotify()
    {
        if (_isProcessing) return;
        _isProcessing = true;

        TryEarnGold(transform.position);
        
        EventBus.Publish(new PropBeingDestroyEvent(transform.position, _prop.Point, _prop.Prefab.transform.localScale));

        Destroy(gameObject);
    }
    
    private void TryEarnGold(Vector3 midPoint)
    {
        if (Random.Range(0f, 100f) <= _prop.GoldChance)
        {
            EventBus.Publish(new GoldEarnEvent(_prop.GoldAmount, midPoint));
        }
    }
}
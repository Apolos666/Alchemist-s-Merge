using UnityEngine;

public class AnimatedVFXController : MonoBehaviour, IPoolable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationName;
    [SerializeField] private string _spawnTriggerName;
    [SerializeField] private string _despawnTriggerName;

    private void Awake()
    {
        if (_animator is null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    public void OnSpawn()
    {
        
    }

    public void OnDespawn()
    {
    }
    
    public void PlaySpawnAnimation()
    {
        _animator.Play(_animationName);
    }
    
    public void CompletedSpawnAnimation()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }
}

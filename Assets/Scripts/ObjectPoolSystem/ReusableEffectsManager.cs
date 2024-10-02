using UnityEngine;

public class ReusableEffectsManager : MonoBehaviour
{
    [Header("VFX Settings")]
    [SerializeField] private AnimatedVFXController _vfxController;
    [SerializeField] private int _vfxPoolSize = 10;
    [SerializeField] private Transform _vfxParent;
    [SerializeField, Tooltip("Additional scale to be applied when merging objects.")] private Vector3 _additionalScale;
    
    [Header("Floating Text Settings")]
    [SerializeField] private FloatingTextController _floatingTextController;
    [SerializeField] private int _floatingTextPoolSize = 10;
    [SerializeField] private Transform _floatingTextParent;

    private void OnEnable()
    {
        EventBus.Subscribe<ObjectMergingEvent>(OnObjectMerging);
        EventBus.Subscribe<PropBeingDestroyEvent>(OnPropBeingDestroy);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ObjectMergingEvent>(OnObjectMerging);
        EventBus.Unsubscribe<PropBeingDestroyEvent>(OnPropBeingDestroy);
    }

    private void Start()
    {
        ObjectPoolManager.Instance.CreatePool(_vfxController, _vfxParent, _vfxPoolSize);
        ObjectPoolManager.Instance.CreatePool(_floatingTextController, _floatingTextParent, _floatingTextPoolSize);
    }
    
    private void OnPropBeingDestroy(PropBeingDestroyEvent message)
    {
        var vfxController = ObjectPoolManager.Instance.Spawn<AnimatedVFXController>(message.MergePosition, Quaternion.identity, message.NextPropSize + _additionalScale);
        vfxController.PlaySpawnAnimation();
        
        var floatTextController = ObjectPoolManager.Instance.Spawn<FloatingTextController>(message.MergePosition, Quaternion.identity);
        floatTextController.SetText(message.Point.ToString());
        
        SoundEffectManager.Instance.PlaySound("MergedObject", message.MergePosition);
    }
    
    private void OnObjectMerging(ObjectMergingEvent message)
    {
        var vfxController = ObjectPoolManager.Instance.Spawn<AnimatedVFXController>(message.MergePosition, Quaternion.identity, message.NextPropSize + _additionalScale);
        vfxController.PlaySpawnAnimation();
        
        var floatTextController = ObjectPoolManager.Instance.Spawn<FloatingTextController>(message.MergePosition, Quaternion.identity);
        floatTextController.SetText(message.Point.ToString());
        
        SoundEffectManager.Instance.PlaySound("MergedObject", message.MergePosition);
    }
}

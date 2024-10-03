using DG.Tweening;
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
    [SerializeField] private Vector3 _floatingTextOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private float _floatingTextDelay = 0.2f;


    private void OnEnable()
    {
        EventBus.Subscribe<ObjectMergingEvent>(OnObjectMerging);
        EventBus.Subscribe<PropBeingDestroyEvent>(OnPropBeingDestroy);
        EventBus.Subscribe<GoldEarnEvent>(OnGoldEarned);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ObjectMergingEvent>(OnObjectMerging);
        EventBus.Unsubscribe<PropBeingDestroyEvent>(OnPropBeingDestroy);
        EventBus.Unsubscribe<GoldEarnEvent>(OnGoldEarned);
    }

    private void Start()
    {
        ObjectPoolManager.Instance.CreatePool(_vfxController, _vfxParent, _vfxPoolSize);
        ObjectPoolManager.Instance.CreatePool(_floatingTextController, _floatingTextParent, _floatingTextPoolSize);
    }
    
    private void OnGoldEarned(GoldEarnEvent message)
    {
        DOVirtual.DelayedCall(_floatingTextDelay, () =>
        {
            var floatTextController = ObjectPoolManager.Instance.Spawn<FloatingTextController>(message.Position + _floatingTextOffset, Quaternion.identity);
            floatTextController.SetText("Coin:", message.Amount.ToString());
        });
    }
    
    private void OnPropBeingDestroy(PropBeingDestroyEvent message)
    {
        var vfxController = ObjectPoolManager.Instance.Spawn<AnimatedVFXController>(message.MergePosition, Quaternion.identity, message.NextPropSize + _additionalScale);
        vfxController.PlaySpawnAnimation();
        
        var floatTextController = ObjectPoolManager.Instance.Spawn<FloatingTextController>(message.MergePosition, Quaternion.identity);
        floatTextController.SetText("Merged:", message.Point.ToString());
        
        SoundEffectManager.Instance.PlaySound("MergedObject", message.MergePosition);
    }
    
    private void OnObjectMerging(ObjectMergingEvent message)
    {
        var vfxController = ObjectPoolManager.Instance.Spawn<AnimatedVFXController>(message.MergePosition, Quaternion.identity, message.NextPropSize + _additionalScale);
        vfxController.PlaySpawnAnimation();
        
        var floatTextController = ObjectPoolManager.Instance.Spawn<FloatingTextController>(message.MergePosition, Quaternion.identity);
        floatTextController.SetText("Merged:", message.Point.ToString());
        
        SoundEffectManager.Instance.PlaySound("MergedObject", message.MergePosition);
    }
}

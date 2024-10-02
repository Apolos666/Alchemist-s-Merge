using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropSelector : GenericSingleton<PropSelector>
{
    [SerializeField, Tooltip("Array of all possible props that can be selected.")]
    private Prop[] _props;
    [SerializeField, Tooltip("Base spawn weights for each prop, determining their initial spawn probability.")]
    private float[] _baseSpawnWeights;
    [SerializeField, Tooltip("Initial number of props that are unlocked at the start.")]
    private int _initialUnlockedProps = 1;
    [SerializeField, Tooltip("Array of point thresholds required to unlock each subsequent prop.")]
    private int[] _unlockThresholds;
    [SerializeField, Tooltip("Multiplier used to adjust the spawn weight of newly unlocked props.")]
    private float _newPropWeightMultiplier = 0.5f;
    
    private UnlockSystem _unlockSystem;
    private readonly Queue<Prop> _propQueue = new Queue<Prop>();
    private const int QueueSize = 2;

    public Prop CurrentProp => _propQueue.Count > 0 ? _propQueue.Peek() : null;

    private void Start()
    {
        _unlockSystem = new UnlockSystem(_unlockThresholds, _initialUnlockedProps, GetPropByIndex);
        InitializePropQueue();
    }

    private Prop GetPropByIndex(int index)
    {
        if (index >= 0 && index < _props.Length)
        {
            return _props[index];
        }

        return null;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<ObjectMergingEvent>(OnScoreUpdate);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ObjectMergingEvent>(OnScoreUpdate);
    }
    
    private void OnScoreUpdate(ObjectMergingEvent message)
    {
        // Khi điểm số thay đổi, cộng thêm điểm để cập nhật cấp độ mở khóa
        _unlockSystem.AddPoints(message.Point);
    }

    private void InitializePropQueue()
    {
        for (var i = 0; i < QueueSize; i++)
        {
            AddNewPropToQueue();
        }
    }

    private void AddNewPropToQueue()
    {
        var randomProp = GetRandomProp();
        EventBus.Publish(new AddNewPropToQueue(randomProp));
        _propQueue.Enqueue(randomProp);
    }

    private Prop GetRandomProp()
    {
        // Bước 1: Lấy danh sách props đã mở khóa
        // Chỉ xem xét các props từ index 0 đến CurrentUnlockLevel - 1
        var unlockedProps = _props.Take(_unlockSystem.CurrentUnlockLevel).ToList();
        var unlockedWeights = _baseSpawnWeights.Take(_unlockSystem.CurrentUnlockLevel).ToList();

        // Bước 2: Điều chỉnh trọng số cho các props mới mở khóa
        for (var i = 0; i < unlockedWeights.Count; i++)
        {
            // Tính số cấp độ đã qua kể từ khi prop này được mở khóa
            // Prop mới nhất sẽ có levelsFromUnlock = 0, prop cũ nhất sẽ có giá trị cao nhất
            var levelsFromUnlock = _unlockSystem.CurrentUnlockLevel - i - 1;
        
            if (levelsFromUnlock >= 0)
            {
                // Giảm trọng số cho props mới mở khóa
                // Công thức: newWeight = baseWeight * (1 / newPropWeightMultiplier)^levelsFromUnlock
                // Ví dụ: nếu newPropWeightMultiplier = 0.5
                // - Prop mới nhất (levelsFromUnlock = 0): không thay đổi
                // - Prop cũ hơn 1 cấp (levelsFromUnlock = 1): trọng số tăng gấp 2 lần
                // - Prop cũ hơn 2 cấp (levelsFromUnlock = 2): trọng số tăng gấp 4 lần, v.v.
                unlockedWeights[i] *= Mathf.Pow(1 / _newPropWeightMultiplier, levelsFromUnlock);
            }
        }

        // Bước 3: Chọn prop ngẫu nhiên dựa trên trọng số đã điều chỉnh
        // Tính tổng trọng số của các props đã mở khóa
        var totalWeight = unlockedWeights.Sum();
        var randomValue = Random.Range(0f, totalWeight);

        // Duyệt qua danh sách props đã mở khóa
        for (var i = 0; i < unlockedProps.Count; i++)
        {
            // Kiểm tra xem giá trị ngẫu nhiên có nằm trong khoảng trọng số của prop hiện tại không
            // Nếu randomValue nhỏ hơn hoặc bằng trọng số của prop hiện tại,
            // thì chọn và trả về prop đó
            if (randomValue <= unlockedWeights[i])
            {
                return unlockedProps[i];
            }
            // Nếu randomValue lớn hơn trọng số của prop hiện tại,
            // trừ trọng số của prop hiện tại khỏi randomValue
            // để kiểm tra prop tiếp theo
            randomValue -= unlockedWeights[i];
        }

        // Trường hợp hiếm khi xảy ra lỗi: nếu không prop nào được chọn (có thể do tính toán sai),
        return unlockedProps[0];
    }

    public (GameObject, Prop) GetNextProp(Vector3 spawnPoint)
    {
        var prop = _propQueue.Dequeue();
        var newProp = Instantiate(prop.Prefab, spawnPoint, Quaternion.identity);
        
        if (_propQueue.Count > 0)
            EventBus.Publish(new NextPropReadyEvent(_propQueue.Peek()));
        
        AddNewPropToQueue();
        return (newProp, prop);
    }

    public void ApplyEnchantedSeed(EnchantedProp enchantedProp)
    {
        if (_propQueue.Count > 0)
        {
            _propQueue.Dequeue();
            _propQueue.Enqueue(enchantedProp);
            
            EventBus.Publish(new NextPropReadyEvent(_propQueue.Peek()));
        }
    }
}
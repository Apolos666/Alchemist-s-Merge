using System;
using UnityEngine;

public class UnlockSystem 
{
    // Mảng chứa các ngưỡng điểm để mở khóa từng cấp độ.
    private readonly int[] _unlockThresholds;
    // Cấp độ mở khóa hiện tại. Giá trị này sẽ tăng khi đạt đủ điểm.
    public int CurrentUnlockLevel { get; private set; }
    // Số điểm hiện tại của người chơi, tích lũy qua quá trình chơi.
    private int _currentPoints;
    // Hàm trả về icon của prop mở khóa theo cấp độ.
    private readonly Func<int, Prop> _getUnlockProp;
    
    /// <summary>
    /// Khởi tạo hệ thống mở khóa với các ngưỡng điểm và cấp độ mở khóa ban đầu.
    /// </summary>
    /// <param name="thresholds">Mảng chứa các ngưỡng điểm số cần thiết để mở khóa các cấp độ props tiếp theo.</param>
    /// <param name="initialUnlockLevel">Cấp độ props ban đầu được mở khóa khi bắt đầu trò chơi.</param>
    public UnlockSystem(int[] thresholds, int initialUnlockLevel, Func<int, Prop> getUnlockProp)
    {
        _unlockThresholds = thresholds;
        CurrentUnlockLevel = initialUnlockLevel;
        _getUnlockProp = getUnlockProp;
        _currentPoints = 0;
    }

    // Phương thức cộng điểm vào hệ thống.
    // Sau khi cộng điểm, sẽ kiểm tra xem có đủ để mở khóa cấp độ mới không.
    public void AddPoints(int points)
    {
        _currentPoints += points;
        CheckForUnlocks();
    }

    // Phương thức kiểm tra xem có mở khóa cấp độ mới không.
    // Nếu đủ điểm cho cấp độ tiếp theo, cấp độ sẽ được mở khóa.
    private void CheckForUnlocks()
    {
        // Kiểm tra cho tới khi đạt tới cấp độ cao nhất hoặc không còn đủ điểm để mở khóa.
        while (CurrentUnlockLevel < _unlockThresholds.Length && _currentPoints >= _unlockThresholds[CurrentUnlockLevel])
        {
            CurrentUnlockLevel++;
            OnPropUnlocked();
        }
    }
    
    public void ResetUnlocks(int initialUnlockLevel)
    {
        CurrentUnlockLevel = initialUnlockLevel;
        _currentPoints = 0;
    }

    private void OnPropUnlocked()
    {
        var prop = _getUnlockProp(CurrentUnlockLevel - 1);
        EventBus.Publish(new PropUnlockedEvent(CurrentUnlockLevel, prop));
    }
}
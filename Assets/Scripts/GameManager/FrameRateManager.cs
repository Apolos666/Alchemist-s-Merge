using UnityEngine;

public class FrameRateManager : GenericPersistentSingleton<FrameRateManager>
{
    [SerializeField] private int _targetFrameRate = 60;

    protected override void Awake()
    {
        base.Awake();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _targetFrameRate;
    }
}
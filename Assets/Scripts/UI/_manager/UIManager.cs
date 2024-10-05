using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : GenericSingleton<UIManager>
{
    private int _activeOverlayCount = 0;

    public void RegisterOverlay(bool isActive)
    {
        if (isActive)
        {
            _activeOverlayCount++;
            if (_activeOverlayCount == 1)
            {
                EventBus.Publish(new UIStateChangedEvent(true));
            }
        }
        else
        {
            _activeOverlayCount = Mathf.Max(0, _activeOverlayCount - 1);
            if (_activeOverlayCount == 0)
            {
                EventBus.Publish(new UIStateChangedEvent(false));
            }
        }
    }
}
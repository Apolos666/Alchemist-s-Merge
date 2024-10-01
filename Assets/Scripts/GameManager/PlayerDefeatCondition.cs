using UnityEngine;

public class PlayerDefeatCondition : MonoBehaviour
{
    private bool _isDefeated;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDefeated) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            _isDefeated = true;
            EventBus.Publish(new PlayerDefeatEvent());
        }
    }
}

public class PlayerDefeatEvent : IEvent
{
    public PlayerDefeatEvent() { }
}
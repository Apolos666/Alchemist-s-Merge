using UnityEngine;

public class SetLayerWhenTrigger : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Item");
            PropManager.Instance.RegisterProp(other.gameObject);
        }
    }
}
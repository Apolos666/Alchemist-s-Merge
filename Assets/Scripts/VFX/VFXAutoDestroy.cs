using UnityEngine;

public class VFXAutoDestroy : MonoBehaviour
{
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}

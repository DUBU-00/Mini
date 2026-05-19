using UnityEngine;

public class DashEffectDestroy : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }
}

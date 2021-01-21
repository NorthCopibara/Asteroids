using UnityEngine;

public class DestructionAsteroidView : MonoBehaviour
{
    public void Despawn() 
    {
        PoolManager.ReleaseObject(gameObject);
    }
}

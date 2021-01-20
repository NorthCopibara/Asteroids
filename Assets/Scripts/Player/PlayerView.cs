using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;

    public Rigidbody2D Rb => _rb;

    public PlayerView Init(Transform spawnPosition) 
    {
        return Instantiate(this, spawnPosition.position, Quaternion.identity);
    }
}

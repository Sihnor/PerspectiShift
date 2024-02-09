using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public enum EPlatform
    {
        Platform1,
        Platform2
    }

    [SerializeField] public EPlatform PlatformNumber;
    
    
    void OnCollisionEnter(Collision collision)
    {
        transform.parent.GetComponent<Teleport>().CollisionDetected(collision, this.PlatformNumber);
    }

}

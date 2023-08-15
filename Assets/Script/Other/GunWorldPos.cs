using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWorldPos : MonoBehaviour
{
    public Transform gun;
    void Update()
    {
        transform.SetLocalPositionAndRotation(gun.position, gun.rotation);
    }
}

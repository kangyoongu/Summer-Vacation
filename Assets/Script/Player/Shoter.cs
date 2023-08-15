using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoter : MonoBehaviour
{
    public Transform shotPoint;
    [SerializeField]
    private GameObject boom;
    public float Power = 100;
    private Rigidbody rigidBody;
    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (GameManager.Instance.isPlay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shot();
            }
        }
    }
    private void Shot()
    {
        GameObject b = Instantiate(boom, shotPoint.position, shotPoint.rotation);
        Rigidbody rigid = b.GetComponent<Rigidbody>();
        rigid.velocity = rigidBody.velocity;
        rigid.AddRelativeForce(Vector3.forward * Power, ForceMode.Impulse);
    }
}

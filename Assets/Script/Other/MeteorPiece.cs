using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorPiece : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(End());
    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(1.5f);
        MarchingCubes.Instance.MakeGround(transform.position, 1);
        Destroy(gameObject);
    }
}

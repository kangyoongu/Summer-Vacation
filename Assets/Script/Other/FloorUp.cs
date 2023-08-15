using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorUp : MonoBehaviour
{
    public float start;
    public float end;
    public float upspeed;
    public Transform pos;
    float time = 0;
    float speed = 0;
    [HideInInspector] public static float y = 0;
    private void Start()
    {
        speed = start;
        y = transform.position.y;
    }
    void Update()
    {
        y = transform.position.y;
        if (GameManager.Instance.isPlay)
        {
            time += Time.deltaTime;
            if (time >= 4)
            {
                transform.Translate(Vector3.up * Time.deltaTime * speed);
                speed += Time.deltaTime * upspeed;
                speed = Mathf.Min(speed, end);
            }
            pos.position = new Vector3(PlayerMove.playerTrm.position.x, y, PlayerMove.playerTrm.position.z);
        }
    }
}

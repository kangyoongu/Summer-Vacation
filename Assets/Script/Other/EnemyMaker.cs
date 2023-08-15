using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaker : MonoBehaviour
{
    float time = 0;
    public float spwanTime = 3;
    public GameObject meteor;
    [Space()]
    public float sspwanTime = 1;
    public GameObject smallMeteor;
    float stime = 0;
    void Update()
    {
        if (GameManager.Instance.isPlay)
        {
            time += Time.deltaTime;
            if (time > spwanTime)
            {
                Instantiate(meteor, PlayerMove.playerTrm.position, Quaternion.identity);
                time = 0;
            }
            stime += Time.deltaTime;
            if (stime > sspwanTime)
            {
                Instantiate(smallMeteor, new Vector3(Random.Range(-17.7f, 17.7f), PlayerMove.playerTrm.position.y + 100, Random.Range(-17.7f, 17.7f)), Quaternion.identity);
                stime = 0;
            }
        }
    }
}

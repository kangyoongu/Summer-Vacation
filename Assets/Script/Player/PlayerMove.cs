using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class PlayerMove : MonoBehaviour
{
    public float Speed = 10.0f;
    float h, v;
    private Rigidbody rigid;
    public float jump;
    public Animator anim;
    public static Transform playerTrm;
    public Transform ceiling;
    public TextMeshProUGUI high;
    public Image background;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        playerTrm = transform;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isPlay)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            anim.SetFloat("x", h);
            anim.SetFloat("y", v);

            Vector3 locVel = transform.InverseTransformDirection(rigid.velocity);
            locVel.x = h * Speed;
            locVel.z = v * Speed;
            rigid.velocity = transform.TransformDirection(locVel);
        }
    }
    private void Update()
    {
        if (GameManager.Instance.isPlay)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
                {
                    if (Jump.canJump == true)
                    {
                        anim.SetInteger("Jump", 1);
                    }
                }
            }
            ceiling.position = new Vector3(0, transform.position.y + 150, 0);
            high.text = (transform.position.y - 1).ToString("0.00m");
        }
    }
    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Lava")
        {
            GameManager.Instance.isPlay = false;
            if(PlayerPrefs.GetFloat("Best") < transform.position.y-1) PlayerPrefs.SetFloat("Best", transform.position.y-1);
            background.DOFade(1, 2).OnComplete(() =>
            {
                SceneManager.LoadScene(0);
            });
        }
    }
    public void Jumps() => rigid.AddForce(Vector3.up * jump, ForceMode.Impulse);
}
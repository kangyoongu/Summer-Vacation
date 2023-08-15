using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public static bool canJump = true;
    public Animator anim;
    public static bool onMap = false;
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            canJump = true;
            if (!other.gameObject.CompareTag("Boom") && anim.GetInteger("Jump") == 1)
            {
                anim.SetInteger("Jump", 2);
            }
            if (other.gameObject.CompareTag("Map"))
            {
                onMap = true;
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
            canJump = true;

        if (other.gameObject.CompareTag("Map"))
        {
            onMap = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && anim.GetInteger("Jump") != 1)
            canJump = false;

        if (other.gameObject.CompareTag("Map"))
        {
            onMap = false;
        }
    }
}

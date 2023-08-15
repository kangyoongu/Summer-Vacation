using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpEnd : MonoBehaviour
{
    private Animator anim;
    public UnityEvent jump;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void Jump()
    {
        jump?.Invoke();
    }
    public void End()
    {
        anim.SetInteger("Jump", 0);
    }
}

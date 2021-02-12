using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimatorController : MonoBehaviour
{
    private Animator _anim;
    public Animator Anim => _anim;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walking = Animator.StringToHash("Walking");
    
    void Start()
    {
        _anim = GetComponent<Animator>();
    }
}

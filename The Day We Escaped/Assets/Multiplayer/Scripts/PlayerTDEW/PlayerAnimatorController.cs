using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private IPlayerMovement _playerMovement;
    private Animator _animator;
    private static readonly int SpeedXAnimId = Animator.StringToHash("SpeedX");
    private static readonly int SpeedZAnimId = Animator.StringToHash("SpeedZ");


    [SerializeField] private int _animState;
    
    [SerializeField] private Vector3 _debugVelocity;
    
    void Start()
    {
        _playerMovement = GetComponent<IPlayerMovement>();
        _animator = GetComponentInChildren<Animator>(true);
    }

    private void Update()
    {
        _animator.SetFloat(SpeedXAnimId, _playerMovement.Direction.x);
        _animator.SetFloat(SpeedZAnimId, _playerMovement.Direction.z);

    }
}

using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementKinematic : MonoBehaviour, IPlayerMovement
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private bool _enabled = true;
    
    private Rigidbody _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public bool Move(Vector3 direction)
    {
        if (!_enabled)
            return false;
        
        _direction = transform.InverseTransformDirection(direction);
     
        direction.Normalize();

        _rigid.MovePosition(_rigid.position + direction * _speed * Time.deltaTime);

        return true;
    }

    public bool Rotate(float yaw)
    {
        if (!_enabled)
            return false;
        
        _rigid.MoveRotation(Quaternion.Euler(Vector3.up * yaw));
        return true;
    }

    public Vector3 Velocity => _direction.normalized;
    public Vector3 Direction => _direction;
    
    public void SetPosition(Vector3 pos)
    {
        _rigid.MovePosition(pos);
    }

    public void SetRotation(Quaternion rot)
    {
        _rigid.MoveRotation(rot);
    }

    public void SetEnabled(bool val)
    {
        _enabled = val;
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        private Rigidbody _rb;
        private Vector3 _eulerAngleVelocity = new Vector3(0, 100, 0);

        [SerializeField] private float _mouseSensitivityX = 90f;
        [SerializeField] private float _mouseSensitivityY = 90f;
        [SerializeField] private bool _isInverted = true;

        [SerializeField] private Vector3 _peakPosition;
        [SerializeField] private float _peakSpeed = 1f;

        private int _inversionValue = -1;

        private Animator anim;


        private void Start()
        {
            _rb = GetComponentInParent<Rigidbody>();
            if (_isInverted)
                _inversionValue = 1;
            else
                _inversionValue = -1;

            anim = GetComponent<Animator>();

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            rotateCamera();
            //sidePeakCamera();
            zoomIn();
        }

        private void rotateCamera()
        {
            float lookHoriz = Input.GetAxisRaw("Mouse Y") * _mouseSensitivityX * Time.deltaTime;
            float lookVert = Input.GetAxisRaw("Mouse X") * _mouseSensitivityY * Time.deltaTime;

            //Horizontal
            Quaternion deltaRot = Quaternion.Euler(new Vector3(0, lookVert, 0));
            _rb.MoveRotation(_rb.rotation * deltaRot);

            //Vertical 
            transform.Rotate(Vector3.right * lookHoriz * _inversionValue);

            //Dont forget to clamp the rotation
        }

        private void zoomIn()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

            }
        }

        private void sidePeakCamera()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                anim.SetFloat("Blend", -_peakSpeed);
            }

            if (Input.GetKey(KeyCode.E))
            {
                anim.SetFloat("Blend", _peakSpeed);
            }
        }
    }
}
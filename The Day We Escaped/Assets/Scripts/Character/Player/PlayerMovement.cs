using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody _rb;
        private Camera _cam;
        [Range(0.5f, 15f)] [SerializeField] private float _moveSpeed = 3f;
        [Range(2f, 15f)] [SerializeField] private float _jumpForce = 5f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            checkMovement();
            checkJump();
            checkCrouch();
        }

        private void checkMovement()
        {
            //Raw to disable the smoothing, that would cause the character to keep moving a few frames.
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            float verticalAxis = Input.GetAxisRaw("Vertical");

            Vector3 movePos = transform.right * horizontalAxis + transform.forward * verticalAxis;
            movePos = movePos.normalized;

            Vector3 newPos = new Vector3(movePos.x * _moveSpeed, _rb.velocity.y, movePos.z * _moveSpeed);

            _rb.velocity = newPos;
        }

        private void checkJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rb.AddForce(new Vector3(0f, _jumpForce, 0f), ForceMode.Impulse);
            }
        }

        private void checkCrouch()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                
            }
        }
    }
}
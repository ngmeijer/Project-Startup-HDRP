using System;
using UnityEngine;

namespace UnityTemplateProjects.PlayerTDEW
{
    public class PlayerTDEWController : Bolt.EntityBehaviour<IPlayerTDWEState>
    {
        [SerializeField] private Transform _renderTransform;

        [SerializeField] private float _pitch;
        [SerializeField] private float _yaw;

        [SerializeField] private bool _forward;
        [SerializeField] private bool _back;

        [SerializeField] private bool _left;
        [SerializeField] private bool _right;

        public float minPitchAngle = -30;
        public float maxPitchAngle = 30;
        
        public bool AcceptMove { get; set; }
        public bool AcceptLook { get; set; }

        private IPlayerMovement _playerMovement;

        private static readonly int SpeedXAnimId = Animator.StringToHash("SpeedX");
        private static readonly int SpeedZAnimId = Animator.StringToHash("SpeedZ");

        private Camera _cam;
        
        public override void Attached()
        {
            AcceptMove = true;
            AcceptLook = true;
            
            state.SetTransforms(state.PlayerTransform, this.transform, _renderTransform);

            _playerMovement = GetComponent<IPlayerMovement>();

            state.SetAnimator(GetComponentInChildren<Animator>());

            if (entity.IsOwner)
            {
                AttacheMainCamera();
            }
        }

        public override void SimulateOwner()
        {
            if (AcceptMove == true)
            {
                UpdateMoveInputs();
            }

            if (AcceptLook)
            {
                UpdateLookInput();
            }

            Vector3 direction = Vector3.zero;

            if (_forward)
            {
                direction += transform.forward;
            }
            else if (_back)
            {
                direction += -1 * transform.forward;
            }

            if (_left)
            {
                direction += -1 * transform.right;
            }
            else if (_right)
            {
                direction += transform.right;
            }
            
            _playerMovement.Rotate(_yaw);
            _playerMovement.Move(direction);

            UpdatePitch();
            
            UpdateAnimator();
        }

        private void UpdatePitch()
        {
            _cam.transform.localRotation = Quaternion.Euler(-1 * _pitch, 0, 0);
        }

        void UpdateAnimator()
        {
            state.Animator.SetFloat(SpeedXAnimId, _playerMovement.Direction.x);
            state.Animator.SetFloat(SpeedZAnimId, _playerMovement.Direction.z);
        }

        /// <summary>
        /// Refactor needs
        /// </summary>
        void UpdateMoveInputs()
        {
            _forward = Input.GetAxis("Vertical") > 0;
            _back = Input.GetAxis("Vertical") < 0;

            _right = Input.GetAxis("Horizontal") > 0;
            _left = Input.GetAxis("Horizontal") < 0;
        }

        void UpdateLookInput()
        {
            _pitch += Input.GetAxis("Mouse Y");
            _pitch = Mathf.Clamp(_pitch, minPitchAngle, maxPitchAngle);

            _yaw += Input.GetAxis("Mouse X");
            _yaw %= 360f;
        }
        
        public void AttacheMainCamera()
        {
            _cam = Camera.main;

            var camParent = this.transform.Find("Camera Pos");
            if (camParent != null)
            {
                _cam.transform.parent = camParent;
                _cam.transform.localPosition = Vector3.zero;
                _cam.transform.localRotation = Quaternion.identity;
            }
            else
            {
                _cam.transform.parent = this.transform;
                _cam.transform.localPosition = new Vector3(0, 1.352f, 0.058f);
                _cam.transform.localRotation = Quaternion.identity;
            }
        }

        public void SetPositionAndRotation(Vector3 pos, Quaternion rot = default)
        {
            _playerMovement.SetPosition(pos);
            _yaw = rot.eulerAngles.y;
            _playerMovement.SetRotation(rot);
        }
    }
}
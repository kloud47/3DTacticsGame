using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [Obsolete("Obsolete")] // meaning it's no longer recommended for use and may be removed in future versions.
    public class CameraController : MonoBehaviour
    {
        private const float MIN_FOLLOW_Y_OFFSET = 2f;
        private const float MAX_FOLLOW_Y_OFFSET = 12f;
        [FormerlySerializedAs("VirtualCamera")] [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private Vector3 TargetFollowOffset;
        private CinemachineTransposer cinemachineTransposer;

        private void Start()
        {
            cinemachineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            TargetFollowOffset = cinemachineTransposer.m_FollowOffset;
        }

        private void Update()
        {
            HandleCameraMovement();
            HandleCameraRotation();
            HandleCameraZoom();
        }

        private void HandleCameraZoom()
        {
            float zoomAmount = 1f;
            if (Input.mouseScrollDelta.y > 0)
            {
                TargetFollowOffset.y -= zoomAmount;
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                TargetFollowOffset.y += zoomAmount;
            }
            TargetFollowOffset.y = Mathf.Clamp(TargetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
            float zoomSpeed = 5f;
            cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, TargetFollowOffset, Time.deltaTime * zoomSpeed);
        }

        private void HandleCameraRotation()
        {
            Vector3 rotationVector = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.Q))
            {
                rotationVector.y = +1f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotationVector.y = -1f;
            }

            float rotationSpeed = 100f;
            transform.eulerAngles += rotationVector * (rotationSpeed * Time.deltaTime);
        }

        private void HandleCameraMovement()
        {
            Vector3 inputMoveDir = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                inputMoveDir.z = +1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputMoveDir.z = -1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputMoveDir.x = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputMoveDir.x = +1f;
            }

            float moveSpeed = 10f;

            Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
            transform.position += moveVector * (moveSpeed * Time.deltaTime);
        }
    }
}

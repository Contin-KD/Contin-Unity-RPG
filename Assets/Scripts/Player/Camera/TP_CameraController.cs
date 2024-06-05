using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TP_CameraController : MonoBehaviour
{
        private CharacterInputSystem _playerInput;

        [SerializeField] private Transform LookAttarGet;
        private Transform playerCamera;     

        
        [Range(0.1f, 1.0f),SerializeField,Header("鼠标灵敏度")] public float mouseInputSpeed;
        [Range(0.1f, 0.5f),SerializeField,Header("相机旋转平滑度")] public float rotationSmoothTime = 0.12f;

        [SerializeField, Header("相机对于玩家")] private float distancePlayerOffset;
        [SerializeField, Header("相机对于玩家")] private Vector3 offsetPlayer;
        [SerializeField] private Vector2 ClmpCameraRang = new Vector2(-85f, 70f);
        [SerializeField] private float lookAtPlayerLerpTime;

        [SerializeField, Header("锁敌")] private bool isLockOn;
        [SerializeField] private Transform currentTarget;

        
        [SerializeField,Header("相机碰撞")] private Vector2 _cameraDistanceMinMax = new Vector2(0.01f, 3f);
        [SerializeField] private float colliderMotionLerpTime; 

        private Vector3 rotationSmoothVelocity; 
        private Vector3 currentRotation;       
        private Vector3 _camDirection;         
        private float _cameraDistance;       
        private float yaw;                        
        private float pitch;                     

        public LayerMask collisionLayer;       


        
        private void Awake()
        {
            playerCamera = Camera.main.transform;
            _playerInput = LookAttarGet.transform.root.GetComponent<CharacterInputSystem>();
        }

        private void Start()
        {
            _camDirection = transform.localPosition.normalized;

            _cameraDistance = _cameraDistanceMinMax.y;
        }

        private void Update()
        {
            UpdateCursor();
            GetCameraControllerInput();
        }



        private void LateUpdate()
        {
            ControllerCamera();
            CheckCameraOcclusionAndCollision(playerCamera);
            //CameraLockOnTarget();
        }

       
        private void ControllerCamera()
        {
            if (!isLockOn)
            {
                currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
                transform.eulerAngles = currentRotation;
            }

            Vector3 fanlePos = LookAttarGet.position - transform.forward * distancePlayerOffset;
            
            transform.position = Vector3.Lerp(transform.position, fanlePos, lookAtPlayerLerpTime * Time.deltaTime);
        }

        private void GetCameraControllerInput()
        {
            if (isLockOn) return;

            yaw += _playerInput.cameraLook.x * mouseInputSpeed;
            pitch -= _playerInput.cameraLook.y * mouseInputSpeed;
            pitch = Mathf.Clamp(pitch, ClmpCameraRang.x, ClmpCameraRang.y);
        }


       
        private void CheckCameraOcclusionAndCollision(Transform camera)
        {
            Vector3 desiredCamPosition = transform.TransformPoint(_camDirection * 3f);

            if (Physics.Linecast(transform.position, desiredCamPosition, out var hit, collisionLayer))
            {
                _cameraDistance = Mathf.Clamp(hit.distance * .9f, _cameraDistanceMinMax.x, _cameraDistanceMinMax.y);

            }
            else
            {
                _cameraDistance = _cameraDistanceMinMax.y;

            }
            camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, _camDirection * (_cameraDistance - 0.1f), colliderMotionLerpTime * Time.deltaTime);

        }

       

        private void UpdateCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }



        private void CameraLockOnTarget()
        {
            if (!isLockOn) return;

            Vector3 directionOfTarget = ((currentTarget.position + currentTarget.transform.up * .7f) - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionOfTarget.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }


        public void SetLookPlayerTarget(Transform target)
        {
            LookAttarGet = target;
        }
}

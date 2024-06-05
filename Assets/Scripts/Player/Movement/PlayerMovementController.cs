using System;
using UnityEngine;

namespace UGG.Move
{
    public class PlayerMovementController : CharacterMovementBase
    {
        //引用
        private Transform characterCamera;
        private TP_CameraController _tpCameraController;
        
        [SerializeField,Header("相机锁定点")] private Transform standCameraLook;
        [SerializeField]private Transform crouchCameraLook;
        
        //Ref Value
        private float targetRotation;
        private float rotationVelocity;
        
        //LerpTime
        [SerializeField,Header("旋转速度")] private float rotationLerpTime;
        [SerializeField] private float moveDirctionSlerpTime;
        

        //Move Speed
        [SerializeField,Header("移动速度")] private float walkSpeed;
        [SerializeField,Header("移动速度")] private float runSpeed;
        [SerializeField,Header("移动速度")] private float crouchMoveSpeed;
        
        
        [SerializeField,Header("角色胶囊控制(下蹲)")] private Vector3 crouchCenter;
        [SerializeField] private Vector3 originCenter;
        [SerializeField] private Vector3 cameraLookPositionOnCrouch;
        [SerializeField] private Vector3 cameraLookPositionOrigin;
        [SerializeField] private float crouchHeight;
        [SerializeField] private float originHeight;
        [SerializeField] private bool isOnCrouch;
        [SerializeField] private Transform crouchDetectionPosition;
        [SerializeField] private Transform CameraLook;
        [SerializeField] private LayerMask crouchDetectionLayer;
        
        //animationID
        private int crouchID = Animator.StringToHash("Crouch");


        #region 内部函数

        protected override void Awake()
        {
            base.Awake();

            characterCamera = Camera.main.transform.root.transform;
            _tpCameraController = characterCamera.GetComponent<TP_CameraController>();
        }

        protected override void Start()
        {
            base.Start();

            
            cameraLookPositionOrigin = CameraLook.position;
        }

        protected override void Update()
        {
            base.Update();
            
            PlayerMoveDirection();
            
        }

        private void LateUpdate()
        {
            CharacterCrouchControl();
            UpdateMotionAnimation();
            UpdateCrouchAnimation();
            UpdateRollAnimation();
            
        }

        #endregion



        #region 条件

        private bool CanMoveContro()
        {
            return isOnGround && characterAnimator.CheckAnimationTag("Motion") || characterAnimator.CheckAnimationTag("CrouchMotion");
        }

        private bool CanCrouch()
        {
            if (characterAnimator.CheckAnimationTag("Crouch")) return false;
            if (characterAnimator.GetFloat(runID)>.9f) return false;
            
            return true;
        }
        
        
        private bool CanRunControl()
        {
            if (Vector3.Dot(movementDirection.normalized, transform.forward) < 0.75f) return false;
            if (!CanMoveContro()) return false;
           

            return true;
        }

        #endregion
        
        
        private void PlayerMoveDirection()
        {
            
            if (isOnGround && _inputSystem.playerMovement == Vector2.zero)
                movementDirection = Vector3.zero;
            
            if(CanMoveContro()) 
            {
                if (_inputSystem.playerMovement != Vector2.zero)
                {
            
                    targetRotation = Mathf.Atan2(_inputSystem.playerMovement.x, _inputSystem.playerMovement.y) * Mathf.Rad2Deg + characterCamera.localEulerAngles.y;
            
                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationLerpTime);

                    var direction = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
            
                    direction = direction.normalized;

                    movementDirection = Vector3.Slerp(movementDirection, ResetMoveDirectionOnSlop(direction),
                        moveDirctionSlerpTime * Time.deltaTime);

                }
            }
            else 
            {
                movementDirection = Vector3.zero;
            }
            
            control.Move((characterCurrentMoveSpeed * Time.deltaTime)
                * movementDirection.normalized + Time.deltaTime
                * new Vector3(0.0f, verticalSpeed, 0.0f));

            
        }


        private void UpdateMotionAnimation()
        {

            if (CanRunControl())
            {
                characterAnimator.SetFloat(movementID,_inputSystem.playerMovement.sqrMagnitude *((_inputSystem.playerRun && !isOnCrouch) ? 2f : 1f),0.1f,Time.deltaTime);
                
                characterCurrentMoveSpeed = (_inputSystem.playerRun && !isOnCrouch) ? runSpeed : walkSpeed;
            }
            else
            {
                characterAnimator.SetFloat(movementID,0f,0.05f,Time.deltaTime);
                characterCurrentMoveSpeed = 0f;
            }

            characterAnimator.SetFloat(runID, (_inputSystem.playerRun && !isOnCrouch) ? 1f : 0f);
        }

        private void UpdateCrouchAnimation()
        {
            if (isOnCrouch)
            {
                characterCurrentMoveSpeed = crouchMoveSpeed;
            }
            
        }

        private void UpdateRollAnimation()
        {
            
        }
        
        private void CharacterCrouchControl()
        {
            if(!CanCrouch()) return;

            if (_inputSystem.playerCrouch)
            {
                
                if (isOnCrouch)
                {
                    if (!DetectionHeadHasObject())
                    {
                        isOnCrouch = false;
                        characterAnimator.SetFloat(crouchID,0f);
                        SetCrouchColliderHeight(originHeight,originCenter);
                        _tpCameraController.SetLookPlayerTarget(standCameraLook);
                    }
                    
                }
                else
                {
                    isOnCrouch = true;
                    characterAnimator.SetFloat(crouchID,1f);
                    SetCrouchColliderHeight(crouchHeight,crouchCenter);
                    _tpCameraController.SetLookPlayerTarget(crouchCameraLook);
                }
            }
        }
        
        
        private void SetCrouchColliderHeight(float height,Vector3 center)
        {
            control.center = center;
            control.height = height;
            
        }
        
        
        private bool DetectionHeadHasObject()
        {
            Collider[] hasObjects = new Collider[1];
            
            int objectCount = Physics.OverlapSphereNonAlloc(crouchDetectionPosition.position, 0.5f, hasObjects,crouchDetectionLayer);
            
            if (objectCount > 0)
            {
                return true;
            }

            return false;
        }
        
    }
    
    
}

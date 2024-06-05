using Unity.Collections;
using UnityEngine;


namespace UGG.Move
{
    //Base class for all roles 角色基类(所有角色，玩家 . 敌人)
    public abstract class CharacterMovementBase : MonoBehaviour
    {
        //引用
        protected Animator characterAnimator;
        protected CharacterController control;
        protected CharacterInputSystem _inputSystem;
        
        //MoveDirection(移动向量)
        protected Vector3 movementDirection;
        protected Vector3 verticalDirection;


        [SerializeField,Header("移动速度")] protected float characterGravity;
        [SerializeField] protected float characterCurrentMoveSpeed;
        protected float characterFallTime = 0.15f;
        protected float characterFallOutDeltaTime;
        protected float verticalSpeed; 
        protected float maxVerticalSpeed = 53f;

        
        [SerializeField,Header("地面检测")] protected float groundDetectionRang;
        [SerializeField] protected float groundDetectionOffset;
        [SerializeField] protected float slopRayExtent;
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField,Tooltip("角色动画移动时检测障碍物的层级")] protected LayerMask whatIsObs;
        [SerializeField] protected bool isOnGround;
        
        
        //AnimationID
        protected int animationMoveID = Animator.StringToHash("AnimationMove");
        protected int movementID = Animator.StringToHash("Movement");
        protected int horizontalID = Animator.StringToHash("Horizontal");
        protected int verticalID = Animator.StringToHash("Vertical");
        protected int runID = Animator.StringToHash("Run");

        protected virtual void Awake()
        {
            characterAnimator = GetComponentInChildren<Animator>();
            control = GetComponent<CharacterController>();
            _inputSystem = GetComponent<CharacterInputSystem>();
            

        }
        
        protected virtual void Start()
        {
            characterFallOutDeltaTime = characterFallTime;
        }

        protected virtual void Update()
        {
            CharacterGravity();
            CheckOnGround();
        }

        #region 内部函数
        
        /// <summary>
        /// 角色重力
        /// </summary>
        private void CharacterGravity()
        {
            if (isOnGround)
            {
                
                characterFallOutDeltaTime = characterFallTime;

                if (verticalSpeed < 0.0f)
                {
                    verticalSpeed = -2f;
                }
            }
            else
            {
                if (characterFallOutDeltaTime >= 0.0f)
                {
                    characterFallOutDeltaTime -= Time.deltaTime;
                    characterFallOutDeltaTime = Mathf.Clamp(characterFallOutDeltaTime, 0, characterFallTime);
                }
            }

            if (verticalSpeed < maxVerticalSpeed)
            {
                verticalSpeed += characterGravity * Time.deltaTime;
            }
        }

        /// <summary>
        /// 地面检测
        /// </summary>
        private void CheckOnGround()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundDetectionOffset, transform.position.z);
            isOnGround = Physics.CheckSphere(spherePosition, groundDetectionRang, whatIsGround, QueryTriggerInteraction.Ignore);
            
        }

        private void OnDrawGizmosSelected()
        {
            
            if (isOnGround) 
                Gizmos.color = Color.green;
            else 
                Gizmos.color = Color.red;

            Vector3 position = Vector3.zero;
            
            position.Set(transform.position.x, transform.position.y - groundDetectionOffset,
                transform.position.z);

            Gizmos.DrawWireSphere(position, groundDetectionRang);
            
        }

        /// <summary>
        /// 坡度检测
        /// </summary>
        /// <param name="dir">当前移动方向</param>
        /// <returns></returns>
        protected Vector3 ResetMoveDirectionOnSlop(Vector3 dir)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out var hit, slopRayExtent))
            {
                float newAnle = Vector3.Dot(Vector3.up, hit.normal);
                if (newAnle != 0 && verticalSpeed <= 0)
                {
                    return Vector3.ProjectOnPlane(dir, hit.normal);
                }
            }
            return dir;
        }
        
        protected bool CanAnimationMotion(Vector3 dir)
        {
            return Physics.Raycast(transform.position + transform.up * .5f, dir.normalized * characterAnimator.GetFloat(animationMoveID), out var hit, 1f,whatIsObs);
        }
        
        #endregion

        #region 公共函数
        
        /// <summary>
        /// 移动接口
        /// </summary>
        /// <param name="moveDirection">移动方向</param>
        /// <param name="moveSpeed">移动速度</param>
        public virtual void CharacterMoveInterface(Vector3 moveDirection, float moveSpeed,bool useGravity)
        {
            if (!CanAnimationMotion(moveDirection))
            {
                movementDirection = moveDirection.normalized;
        
                movementDirection = ResetMoveDirectionOnSlop(movementDirection);

                if (useGravity)
                {
                    verticalDirection.Set(0.0f, verticalSpeed, 0.0f);
                }
                else
                {
                    verticalDirection = Vector3.zero;
                }
        
                control.Move((moveSpeed * Time.deltaTime)
                    * movementDirection.normalized + Time.deltaTime
                    * verticalDirection);
            }
        }

        #endregion
        
        
    }
}

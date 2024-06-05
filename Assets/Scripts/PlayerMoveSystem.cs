//using System.Collections;
//using System.Collections.Generic;
//using UGG.Move;
//using UnityEngine;

//// 玩家姿态枚举
//public enum PlayerPosture
//{
//    Squat,  // 蹲下
//    Stand,  // 站立
//    Air     // 空中
//}

//public class PlayerMoveSystem : MonoBehaviour
//{
//    private HuoHuoController controller;

//    // 玩家当前状态和姿态
//    public PlayerPosture curPosture = PlayerPosture.Stand;

//    // 动画参数哈希值
//    private int postureHash;
//    private int speedHash;
//    private int turnHash;
//    private int slideHash;

//    // 姿态阈值
//    private const int squatThreshold = -1;
//    private const int standThreshold = 0;
//    private const int airThreshold = 1;

//    // 速度
//    private float walkSpeed = 5;
//    private Vector2 movement;

//    private bool isSquat = false;
//    private bool isJump = false;
//    private bool isRun = false;
//    private bool isSlide = false;

//    // 摄像机变换
//    public Transform cameraTransform;

//    #region 生命周期
//    private void Awake()
//    {
//        controller = GetComponent<HuoHuoController>();
//    }

//    private void Start()
//    {
//        // 获取动画参数哈希值
//        postureHash = Animator.StringToHash("Posture");
//        speedHash = Animator.StringToHash("Movement");
//        turnHash = Animator.StringToHash("Angle");
//        slideHash = Animator.StringToHash("Slider");
//        controller._animator.SetFloat(postureHash, standThreshold);
//    }

//    private void Update()
//    {
//        // 获取玩家移动输入
//        movement = controller._characterInputSystem.playerMovement;

//        // 切换蹲下姿态
//        if ((curPosture == PlayerPosture.Squat || curPosture == PlayerPosture.Stand) && !isRun)
//        {
//            if (controller._characterInputSystem.playerCrouchClick)
//            {
//                isSquat = !isSquat;
//            }
//        }
//    }

//    private void FixedUpdate()
//    {
//        SetPlayerAnim();
//        SwitchPlayerPosture();
//        RotatePlayerToCameraDirection();
//        HandleInput();
//    }
//    #endregion

//    #region 外部函数
//    // 处理输入
//    private void HandleInput()
//    {
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            UnlockCursor();
//        }

//        if (Input.GetMouseButtonDown(0))
//        {
//            LockCursor();
//        }
//    }

//    // 锁定鼠标
//    private void LockCursor()
//    {
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//    }

//    // 解锁鼠标
//    private void UnlockCursor()
//    {
//        Cursor.lockState = CursorLockMode.None;
//        Cursor.visible = true;
//    }

//    // 将玩家朝向摄像机方向旋转
//    private void RotatePlayerToCameraDirection()
//    {
//        Vector3 direction = transform.position - cameraTransform.position;
//        direction.y = 0;
//        if (direction.sqrMagnitude > 0.001f)
//        {
//            Quaternion targetRotation = Quaternion.LookRotation(direction);
//            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
//            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
//        }
//    }

//    // 设置玩家动画参数
//    private void SetPlayerAnim()
//    {
//        // 如果没有按下蹲键，重置滑动动画参数
//        if (!controller._characterInputSystem.playerCrouch)
//        {
//            controller._animator.SetFloat(slideHash, 0, 0.1f, Time.deltaTime);
//        }

//        // 根据玩家姿态设置动画参数
//        switch (curPosture)
//        {
//            case PlayerPosture.Squat:
//                walkSpeed = 3;
//                break;
//            case PlayerPosture.Stand:
//                walkSpeed = isRun ? 8 : 5;
//                break;
//            case PlayerPosture.Air:
//                // 空中姿态的逻辑在这里处理（如果需要）
//                break;
//        }

//        controller._animator.SetFloat(speedHash, movement.y * walkSpeed, 0.1f, Time.deltaTime);
//        controller._animator.SetFloat(turnHash, movement.x * walkSpeed, 0.1f, Time.deltaTime);

//        if (isRun && controller._characterInputSystem.playerCrouch)
//        {
//            controller._animator.SetFloat(slideHash, 1, 0.05f, Time.deltaTime);
//        }
//    }

//    // 切换玩家姿态
//    private void SwitchPlayerPosture()
//    {
//        if (isSlide)
//        {
//            curPosture = PlayerPosture.Stand;
//            controller._animator.SetFloat(postureHash, standThreshold, 0.1f, Time.deltaTime);
//        }
//        else if (isSquat)
//        {
//            curPosture = PlayerPosture.Squat;
//            controller._animator.SetFloat(postureHash, squatThreshold, 0.1f, Time.deltaTime);
//        }
//        else if (isJump)
//        {
//            curPosture = PlayerPosture.Air;
//            controller._animator.SetFloat(postureHash, airThreshold, 0.1f, Time.deltaTime);
//        }
//        else
//        {
//            curPosture = PlayerPosture.Stand;
//            controller._animator.SetFloat(postureHash, standThreshold, 0.1f, Time.deltaTime);
//            isRun = controller._characterInputSystem.playerRun;
//        }
//    }
//    #endregion
//}
using System.Collections;
using System.Collections.Generic;
using UGG.Move;
using UnityEngine;

// 玩家姿态枚举
public enum PlayerPosture
{
    Squat,  // 蹲下
    Stand,  // 站立
    Air,    // 空中
    Dodge   // 闪避
}

public class PlayerMoveSystem : MonoBehaviour
{
    private HuoHuoController controller;

    // 玩家当前状态和姿态
    public PlayerPosture curPosture = PlayerPosture.Stand;

    // 动画参数哈希值
    private int postureHash;
    private int speedHash;
    private int turnHash;
    private int slideHash;
    private int dodgeHash;

    // 姿态阈值
    private const int squatThreshold = -1;
    private const int standThreshold = 0;
    private const int airThreshold = 1;
    private const int dodgeThreshold = 2;

    // 速度
    private float walkSpeed = 5;
    private Vector2 movement;

    private bool isSquat = false;
    private bool isJump = false;
    private bool isRun = false;
    private bool isSlide = false;
    private bool isDodge = false;

    // 摄像机变换
    public Transform cameraTransform;

    #region 生命周期
    private void Awake()
    {
        controller = GetComponent<HuoHuoController>();
    }

    private void Start()
    {
        // 获取动画参数哈希值
        postureHash = Animator.StringToHash("Posture");
        speedHash = Animator.StringToHash("Movement");
        turnHash = Animator.StringToHash("Angle");
        slideHash = Animator.StringToHash("Slider");
        dodgeHash = Animator.StringToHash("Dodge");
        controller._animator.SetFloat(postureHash, standThreshold);
    }

    private void Update()
    {
        // 获取玩家移动输入
        movement = controller._characterInputSystem.playerMovement;

        //// 切换蹲下姿态
        //if ((curPosture == PlayerPosture.Squat || curPosture == PlayerPosture.Stand) && !isRun)
        //{
        //    if (controller._characterInputSystem.playerCrouchClick)
        //    {
        //        isSquat = !isSquat;
        //    }
        //}

        // 监听 Shift 键的输入
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // 如果 Shift 键按下时间较短，则进行闪避
            StartCoroutine(DodgeCoroutine());
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // 如果长按 Shift 键，则跑步
            isRun = true;
        }
        else
        {
            isRun = false;
        }
    }

    private void FixedUpdate()
    {
        SetPlayerAnim();
        SwitchPlayerPosture();
        RotatePlayerToCameraDirection();
        HandleInput();
    }
    #endregion

    #region 外部函数
    // 处理输入
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }
    }

    // 锁定鼠标
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 解锁鼠标
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 将玩家朝向摄像机方向旋转
    private void RotatePlayerToCameraDirection()
    {
        Vector3 direction = transform.position - cameraTransform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }

    // 设置玩家动画参数
    private void SetPlayerAnim()
    {
        // 如果没有按下蹲键，重置滑动动画参数
        if (!controller._characterInputSystem.playerCrouch)
        {
            controller._animator.SetFloat(slideHash, 0, 0.1f, Time.deltaTime);
        }

        // 根据玩家姿态设置动画参数
        switch (curPosture)
        {
            case PlayerPosture.Squat:
                walkSpeed = 3;
                break;
            case PlayerPosture.Stand:
                walkSpeed = isRun ? 8 : 5;
                controller._animator.SetFloat(speedHash, movement.y * walkSpeed, 0.1f, Time.deltaTime);
                controller._animator.SetFloat(turnHash, movement.x * walkSpeed, 0.1f, Time.deltaTime);
                break;
            case PlayerPosture.Air:
                // 空中姿态的逻辑在这里处理（如果需要）
                break;
            case PlayerPosture.Dodge:
                walkSpeed = 10; // 假设闪避速度较快
                break;
        }

     
        if (isRun && controller._characterInputSystem.playerCrouch)
        {
            controller._animator.SetFloat(slideHash, 1, 0.05f, Time.deltaTime);
        }
    }

    // 切换玩家姿态
    private void SwitchPlayerPosture()
    {
        //if (isDodge)
        //{
        //    curPosture = PlayerPosture.Dodge;
        //    controller._animator.SetFloat(postureHash, dodgeThreshold, 0.1f, Time.deltaTime);
        //}
        //else
        if (isSlide)
        {
            curPosture = PlayerPosture.Stand;
            controller._animator.SetFloat(postureHash, standThreshold, 0.1f, Time.deltaTime);
        }
        else if (isSquat)
        {
            curPosture = PlayerPosture.Squat;
            controller._animator.SetFloat(postureHash, squatThreshold, 0.1f, Time.deltaTime);
        }
        else if (isJump)
        {
            curPosture = PlayerPosture.Air;
            controller._animator.SetFloat(postureHash, airThreshold, 0.1f, Time.deltaTime);
        }
        else
        {
            curPosture = PlayerPosture.Stand;
            controller._animator.SetFloat(postureHash, standThreshold, 0.1f, Time.deltaTime);
        }
    }
    #endregion

    #region 闪避逻辑
    private IEnumerator DodgeCoroutine()
    {
        isDodge = true;
        yield return new WaitForSeconds(0.2f); // 闪避时间持续0.2秒
        isDodge = false;
    }
    #endregion
}

//using System.Collections;
//using System.Collections.Generic;
//using UGG.Move;
//using UnityEngine;

//// �����̬ö��
//public enum PlayerPosture
//{
//    Squat,  // ����
//    Stand,  // վ��
//    Air     // ����
//}

//public class PlayerMoveSystem : MonoBehaviour
//{
//    private HuoHuoController controller;

//    // ��ҵ�ǰ״̬����̬
//    public PlayerPosture curPosture = PlayerPosture.Stand;

//    // ����������ϣֵ
//    private int postureHash;
//    private int speedHash;
//    private int turnHash;
//    private int slideHash;

//    // ��̬��ֵ
//    private const int squatThreshold = -1;
//    private const int standThreshold = 0;
//    private const int airThreshold = 1;

//    // �ٶ�
//    private float walkSpeed = 5;
//    private Vector2 movement;

//    private bool isSquat = false;
//    private bool isJump = false;
//    private bool isRun = false;
//    private bool isSlide = false;

//    // ������任
//    public Transform cameraTransform;

//    #region ��������
//    private void Awake()
//    {
//        controller = GetComponent<HuoHuoController>();
//    }

//    private void Start()
//    {
//        // ��ȡ����������ϣֵ
//        postureHash = Animator.StringToHash("Posture");
//        speedHash = Animator.StringToHash("Movement");
//        turnHash = Animator.StringToHash("Angle");
//        slideHash = Animator.StringToHash("Slider");
//        controller._animator.SetFloat(postureHash, standThreshold);
//    }

//    private void Update()
//    {
//        // ��ȡ����ƶ�����
//        movement = controller._characterInputSystem.playerMovement;

//        // �л�������̬
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

//    #region �ⲿ����
//    // ��������
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

//    // �������
//    private void LockCursor()
//    {
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//    }

//    // �������
//    private void UnlockCursor()
//    {
//        Cursor.lockState = CursorLockMode.None;
//        Cursor.visible = true;
//    }

//    // ����ҳ��������������ת
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

//    // ������Ҷ�������
//    private void SetPlayerAnim()
//    {
//        // ���û�а��¶׼������û�����������
//        if (!controller._characterInputSystem.playerCrouch)
//        {
//            controller._animator.SetFloat(slideHash, 0, 0.1f, Time.deltaTime);
//        }

//        // ���������̬���ö�������
//        switch (curPosture)
//        {
//            case PlayerPosture.Squat:
//                walkSpeed = 3;
//                break;
//            case PlayerPosture.Stand:
//                walkSpeed = isRun ? 8 : 5;
//                break;
//            case PlayerPosture.Air:
//                // ������̬���߼������ﴦ�������Ҫ��
//                break;
//        }

//        controller._animator.SetFloat(speedHash, movement.y * walkSpeed, 0.1f, Time.deltaTime);
//        controller._animator.SetFloat(turnHash, movement.x * walkSpeed, 0.1f, Time.deltaTime);

//        if (isRun && controller._characterInputSystem.playerCrouch)
//        {
//            controller._animator.SetFloat(slideHash, 1, 0.05f, Time.deltaTime);
//        }
//    }

//    // �л������̬
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

// �����̬ö��
public enum PlayerPosture
{
    Squat,  // ����
    Stand,  // վ��
    Air,    // ����
    Dodge   // ����
}

public class PlayerMoveSystem : MonoBehaviour
{
    private HuoHuoController controller;

    // ��ҵ�ǰ״̬����̬
    public PlayerPosture curPosture = PlayerPosture.Stand;

    // ����������ϣֵ
    private int postureHash;
    private int speedHash;
    private int turnHash;
    private int slideHash;
    private int dodgeHash;

    // ��̬��ֵ
    private const int squatThreshold = -1;
    private const int standThreshold = 0;
    private const int airThreshold = 1;
    private const int dodgeThreshold = 2;

    // �ٶ�
    private float walkSpeed = 5;
    private Vector2 movement;

    private bool isSquat = false;
    private bool isJump = false;
    private bool isRun = false;
    private bool isSlide = false;
    private bool isDodge = false;

    // ������任
    public Transform cameraTransform;

    #region ��������
    private void Awake()
    {
        controller = GetComponent<HuoHuoController>();
    }

    private void Start()
    {
        // ��ȡ����������ϣֵ
        postureHash = Animator.StringToHash("Posture");
        speedHash = Animator.StringToHash("Movement");
        turnHash = Animator.StringToHash("Angle");
        slideHash = Animator.StringToHash("Slider");
        dodgeHash = Animator.StringToHash("Dodge");
        controller._animator.SetFloat(postureHash, standThreshold);
    }

    private void Update()
    {
        // ��ȡ����ƶ�����
        movement = controller._characterInputSystem.playerMovement;

        //// �л�������̬
        //if ((curPosture == PlayerPosture.Squat || curPosture == PlayerPosture.Stand) && !isRun)
        //{
        //    if (controller._characterInputSystem.playerCrouchClick)
        //    {
        //        isSquat = !isSquat;
        //    }
        //}

        // ���� Shift ��������
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // ��� Shift ������ʱ��϶̣����������
            StartCoroutine(DodgeCoroutine());
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // ������� Shift �������ܲ�
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

    #region �ⲿ����
    // ��������
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

    // �������
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // �������
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ����ҳ��������������ת
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

    // ������Ҷ�������
    private void SetPlayerAnim()
    {
        // ���û�а��¶׼������û�����������
        if (!controller._characterInputSystem.playerCrouch)
        {
            controller._animator.SetFloat(slideHash, 0, 0.1f, Time.deltaTime);
        }

        // ���������̬���ö�������
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
                // ������̬���߼������ﴦ�������Ҫ��
                break;
            case PlayerPosture.Dodge:
                walkSpeed = 10; // ���������ٶȽϿ�
                break;
        }

     
        if (isRun && controller._characterInputSystem.playerCrouch)
        {
            controller._animator.SetFloat(slideHash, 1, 0.05f, Time.deltaTime);
        }
    }

    // �л������̬
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

    #region �����߼�
    private IEnumerator DodgeCoroutine()
    {
        isDodge = true;
        yield return new WaitForSeconds(0.2f); // ����ʱ�����0.2��
        isDodge = false;
    }
    #endregion
}

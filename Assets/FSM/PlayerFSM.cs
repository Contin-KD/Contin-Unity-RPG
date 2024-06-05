using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PlayerInfo
{
    public Animator animator;
    public CharacterInputSystem inputSystem;
    public CharacterController controller;
}

public class PlayerFSM : MonoBehaviour
{
    private FSM fsm;
    private PlayerInfo playerInfo;
    public StateType curState;
    public Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        // 实例化状态机
        fsm = new FSM();

        playerInfo = new PlayerInfo();
        playerInfo.animator = GetComponent<Animator>();
        playerInfo.inputSystem = GetComponent<CharacterInputSystem>();
        playerInfo.controller = GetComponent<CharacterController>();
        // 添加状态
        InitState();
        fsm.SwitchState(StateType.Idle);
    }

    private void InitState()
    {
        fsm.AddState(StateType.Idle, new IdleState(fsm, playerInfo));
        fsm.AddState(StateType.Walk, new WalkState(fsm, playerInfo));
        fsm.AddState(StateType.Run, new RunState(fsm, playerInfo));
        fsm.AddState(StateType.Crouch, new CrouchState(fsm, playerInfo));
        fsm.AddState(StateType.Sliding, new SlidingState(fsm, playerInfo));
        fsm.AddState(StateType.Dodge, new DodgeState(fsm, playerInfo));
        fsm.AddState(StateType.RAtk, new RAtkState(fsm, playerInfo));
        fsm.AddState(StateType.LAtk, new LAtkState(fsm, playerInfo));
        fsm.AddState(StateType.Jump, new JumpState(fsm, playerInfo));
    }

    private void Update()
    {
        fsm.OnUpdate();
        HandleInput();
        RotatePlayerToCameraDirection();
    }
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
}



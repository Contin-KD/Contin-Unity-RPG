using UnityEngine;

public class JumpState : IStateBase
{
    private FSM fsm;
    private Animator animator;
    private CharacterInputSystem inputSystem;
    private CharacterController characterController;

    private enum JumpPhase
    {
        Start,
        MidAir,
        Land
    }

    private JumpPhase currentPhase;

    public JumpState(FSM fsm, PlayerInfo playerInfo)
    {
        this.fsm = fsm;
        animator = playerInfo.animator;
        inputSystem = playerInfo.inputSystem;
        characterController = playerInfo.controller;
    }

    public void OnEnter()
    {
        // 进入跳跃状态时，初始化为起跳阶段
        currentPhase = JumpPhase.Start;
        animator.SetTrigger("JumpStart");
    }

    public void OnExit()
    {
       
    }

    public void OnUpdate()
    {
        if (characterController.isGrounded)
        {
            fsm.SwitchState(StateType.Idle); // 切换到其他状态
        }
    }

}

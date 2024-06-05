using System.Collections;
using System.Collections.Generic;
using UGG.Move;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : IStateBase
{
    private FSM fsm;
    private Animator animator;
    private CharacterInputSystem inputSystem;
    private int speedHash;
    private int turnHash;

    public IdleState(FSM fsm, PlayerInfo playerInfo)
    {
        this.fsm = fsm;
        animator = playerInfo.animator;
        inputSystem = playerInfo.inputSystem;
        speedHash = Animator.StringToHash("前后");
        turnHash = Animator.StringToHash("左右");
    }

    public void OnEnter()
    {
        Debug.Log("[OnEnter]: 进入Idle状态");
        animator.SetBool("Idle", true);
    }

    public void OnExit()
    {
        animator.SetBool("Idle", false);
    }

    public void OnUpdate()
    {
        InputHandle();
    }

    private void InputHandle()
    {
        if (inputSystem.playerLAtk)
        {
            fsm.SwitchState(StateType.LAtk);
        }
        else if (inputSystem.playerRAtk)
        {
            fsm.SwitchState(StateType.RAtk);
        }
        else if (inputSystem.playerJump)
        {
            fsm.SwitchState(StateType.Jump);
        }
        else if (inputSystem.playerMovement != Vector2.zero)
        {
            fsm.SwitchState(StateType.Walk);
        }
        else if (inputSystem.playerCrouch)
        {
            fsm.SwitchState(StateType.Crouch);
        }
        else
        {
            animator.SetFloat(speedHash, 0, 0.1f, Time.deltaTime);
            animator.SetFloat(turnHash, 0, 0.1f, Time.deltaTime);
        }
    }
}

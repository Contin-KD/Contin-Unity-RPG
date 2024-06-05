using System.Collections;
using UnityEngine;

public class SlidingState : IStateBase
{
    private FSM fsm;
    private Animator animator;
    private CharacterInputSystem inputSystem;
    private Vector2 movement = Vector2.zero;
    private int speedHash;
    private int turnHash;
    public SlidingState(FSM fsm, PlayerInfo playerInfo)
    {
        this.fsm = fsm;
        animator = playerInfo.animator;
        inputSystem = playerInfo.inputSystem;
        speedHash = Animator.StringToHash("前后");
        turnHash = Animator.StringToHash("左右");
    }

    public void OnEnter()
    {
        Debug.Log("[OnEnter]: 进入滑铲状态");

    }

    public void OnExit()
    {
        //animator.SetBool("Sliding", false);
    }

    public void OnUpdate()
    {
        //if (inputSystem.playerCrouch)
        //{
        //    fsm.SwitchState(StateType.Idle);
        //}
        //movement = inputSystem.playerMovement;
        //if (inputSystem.playerCrouch)
        //{
        //    fsm.SwitchState(StateType.Idle);
        //}
        //else if (movement != Vector2.zero)
        //{
        //    animator.SetFloat(speedHash, movement.y, 0.1f, Time.deltaTime);
        //    animator.SetFloat(turnHash, movement.x, 0.1f, Time.deltaTime);
        //}
    }
}
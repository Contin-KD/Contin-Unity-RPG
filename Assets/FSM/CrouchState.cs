using System.Collections;
using UnityEngine;

public class CrouchState : IStateBase
{
    private FSM fsm;
    private Animator animator;
    private CharacterInputSystem inputSystem;
    private Vector2 movement = Vector2.zero;
    private int speedHash;
    private int turnHash;
    public CrouchState(FSM fsm, PlayerInfo playerInfo)
    {
        this.fsm = fsm;
        animator = playerInfo.animator;
        inputSystem = playerInfo.inputSystem;
        speedHash = Animator.StringToHash("前后");
        turnHash = Animator.StringToHash("左右");
    }

    public void OnEnter()
    {
        Debug.Log("[OnEnter]: 进入Crouch状态");
        animator.SetBool("Crouch", true);
    }

    public void OnExit()
    {
        animator.SetBool("Crouch", false);
    }

    public void OnUpdate()
    {
        movement = inputSystem.playerMovement;
        if (inputSystem.playerCrouch)
        {
            fsm.SwitchState(StateType.Idle);
        }
        else if (movement != Vector2.zero)
        {
            animator.SetFloat(speedHash, movement.y, 0.1f, Time.deltaTime);
            animator.SetFloat(turnHash, movement.x, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(speedHash, 0, 0.1f, Time.deltaTime);
            animator.SetFloat(turnHash, 0, 0.1f, Time.deltaTime);
        }
    }
}
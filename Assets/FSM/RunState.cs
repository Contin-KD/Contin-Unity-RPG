using UnityEngine;

public class RunState : IStateBase
{
    private FSM fsm;
    private Animator animator;
    private CharacterInputSystem inputSystem;
    private Vector2 movement = Vector2.zero;
    private int speedHash;
    private int turnHash;

    public RunState(FSM fsm, PlayerInfo playerInfo)
    {
        this.fsm = fsm;
        animator = playerInfo.animator;
        inputSystem = playerInfo.inputSystem;
        speedHash = Animator.StringToHash("前后");
        turnHash = Animator.StringToHash("左右");
    }

    public void OnEnter()
    {
        Debug.Log("[OnEnter]: 进入Run状态");
        animator.SetBool("Run", true);
    }

    public void OnExit()
    {
        Debug.Log("[OnExit]: 退出Run状态");
        animator.SetBool("Run", false);
    }

    public void OnUpdate()
    {
        movement = inputSystem.playerMovement;

        if (!inputSystem.playerRun) // 检测到取消奔跑
        {
            fsm.SwitchState(StateType.Walk);
        }
        else if (inputSystem.playerCrouch) // 检测到滑行
        {
            animator.SetTrigger("Sliding");
        }
        else if (movement != Vector2.zero) // 更新奔跑动画
        {
            animator.SetFloat(speedHash, movement.y, 0.1f, Time.deltaTime);
            animator.SetFloat(turnHash, movement.x, 0.1f, Time.deltaTime);
        }
        else // 如果没有输入，保持奔跑状态
        {
            fsm.SwitchState(StateType.Run);
        }
    }
}

using UnityEngine;

public class WalkState : IStateBase
{
    private FSM fsm;
    private Animator animator;
    private CharacterInputSystem inputSystem;
    private Vector2 movement = Vector2.zero;
    private int speedHash;
    private int turnHash;

    public WalkState(FSM fsm, PlayerInfo playerInfo)
    {
        this.fsm = fsm;
        animator = playerInfo.animator;
        inputSystem = playerInfo.inputSystem;
        speedHash = Animator.StringToHash("前后");
        turnHash = Animator.StringToHash("左右");
    }

    public void OnEnter()
    {
        Debug.Log("[OnEnter]: 进入Walk状态");
        animator.SetBool("Walk", true);
        inputSystem.ResetDodge();
    }

    public void OnExit()
    {
        Debug.Log("[OnExit]: 退出Walk状态");
        animator.SetBool("Walk", false);
    }

    public void OnUpdate()
    {
        movement = inputSystem.playerMovement;
        // 提前检测输入
        if (inputSystem.playerRun)
        {
            fsm.SwitchState(StateType.Run);
            return;
        }
        else if (inputSystem.playerCrouch)
        {
            fsm.SwitchState(StateType.Crouch);
            return;
        }
        else if (inputSystem.playerLAtk)
        {
            fsm.SwitchState(StateType.LAtk);
        }
        else if (inputSystem.playerRAtk)
        {
            fsm.SwitchState(StateType.RAtk);
        }
        else if (movement != Vector2.zero && inputSystem.playerDodge)
        {
            fsm.SwitchState(StateType.Dodge);
        }
        else if (movement != Vector2.zero)
        {
            // 使用平滑过渡参数
            animator.SetFloat(speedHash, movement.y, 0.1f, Time.deltaTime);
            animator.SetFloat(turnHash, movement.x, 0.1f, Time.deltaTime);
        }
        else
        {
            fsm.SwitchState(StateType.Idle);
        }
    }
}

using UnityEngine;

public class DodgeState : IStateBase
{
    private FSM fsm;
    private Animator animator;
    private CharacterInputSystem inputSystem;
    private int speedHash;
    private int turnHash;
    private float dodgeDuration = 0.5f; // 闪避时间
    private float dodgeTimer;
    private Vector2 dodgeDirection;
    private bool isDodging;

    public DodgeState(FSM fsm, PlayerInfo playerInfo)
    {
        this.fsm = fsm;
        animator = playerInfo.animator;
        inputSystem = playerInfo.inputSystem;
        speedHash = Animator.StringToHash("前后");
        turnHash = Animator.StringToHash("左右");
    }

    public void OnEnter()
    {
        Debug.Log("[OnEnter]: 进入Dodge状态");
        isDodging = true;
        dodgeDirection = inputSystem.playerMovement; // 在进入状态时确定闪避方向
        DetermineDodgeDirection();
        dodgeTimer = dodgeDuration;
        inputSystem.ResetDodge(); // 重置闪避输入
    }

    public void OnExit()
    {
        Debug.Log("[OnExit]: 退出Dodge状态");
        isDodging = false;
        animator.SetBool("IsDodging", false);
    }

    public void OnUpdate()
    {
        if (isDodging)
        {
            // 在闪避过程中保持固定的方向
            animator.SetFloat(speedHash, dodgeDirection.y);
            animator.SetFloat(turnHash, dodgeDirection.x);

            dodgeTimer -= Time.deltaTime;
            if (dodgeTimer <= 0)
            {
                isDodging = false;
                // 检查是否返回Idle或Walk状态
                if (inputSystem.playerMovement == Vector2.zero)
                {
                    fsm.SwitchState(StateType.Idle);
                }
                else
                {
                    fsm.SwitchState(StateType.Walk);
                }
            }
        }
    }

    private void DetermineDodgeDirection()
    {
        if (dodgeDirection == Vector2.zero)
        {
            dodgeDirection = new Vector2(0, 1); // 默认前闪避
        }

        animator.SetBool("IsDodging", true);

        if (dodgeDirection.y > 0)
        {
            Debug.Log("前闪避");
            animator.SetTrigger("DodgeForward");
        }
        else if (dodgeDirection.y < 0)
        {
            Debug.Log("后闪避");
            animator.SetTrigger("DodgeBackward");
        }
        else if (dodgeDirection.x > 0)
        {
            Debug.Log("右闪避");
            animator.SetTrigger("DodgeRight");
        }
        else if (dodgeDirection.x < 0)
        {
            Debug.Log("左闪避");
            animator.SetTrigger("DodgeLeft");
        }
    }
}

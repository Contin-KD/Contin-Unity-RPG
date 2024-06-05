using UnityEngine;

public class RAtkState : IStateBase
{
    private FSM fsm;
    private Animator animator;
    private CharacterInputSystem inputSystem;
    private float atkTimer = 0.5f;

    public RAtkState(FSM fsm, PlayerInfo playerInfo)
    {
        this.fsm = fsm;
        animator = playerInfo.animator;
        inputSystem = playerInfo.inputSystem;
    }

    public void OnEnter()
    {
        Debug.Log("[OnEnter]: 进入Atk状态");
        animator.SetTrigger("RAtk");
    }

    public void OnExit()
    {
        Debug.Log("[OnExit]: 退出Atk状态");
    }

    public void OnUpdate()
    {
        atkTimer -= Time.deltaTime;
        if (atkTimer <= 0)
        {
            if (inputSystem.playerMovement == Vector2.zero)
            {
                fsm.SwitchState(StateType.Idle);
            }
            else
            {
                fsm.SwitchState(StateType.Walk);
            }
        }
        if (inputSystem.playerRAtk)
        {
            animator.SetTrigger("RAtk");
        }
    }
}

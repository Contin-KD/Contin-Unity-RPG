using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StateType
{
    Idle,
    Walk,
    Run,
    Crouch,
    Sliding,
    Dodge,
    RAtk,
    LAtk,
    Jump
}

public interface IStateBase
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}

public class FSM
{
    // ��ǰ״̬
    public IStateBase curState;
    public Dictionary<StateType, IStateBase> stateDic;
    public FSM()
    {
        this.stateDic = new Dictionary<StateType, IStateBase>();
    }
    /// <summary>
    /// ���״̬
    /// </summary>
    /// <param name="stateType"></param>
    /// <param name="state"></param>
    public void AddState(StateType stateType, IStateBase state)
    {
        if (stateDic.ContainsKey(stateType))
        {
            Debug.Log("״̬�ظ�");
            return;
        }
        stateDic.Add(stateType, state);
    }
    /// <summary>
    /// ����״̬
    /// </summary>
    /// <param name="stateType"></param>
    public void SwitchState(StateType stateType)
    {
        if (!stateDic.ContainsKey(stateType))
        {
            Debug.Log("û�и�״̬");
        }
        if (curState != null)
        {
            curState.OnExit();
        }
        curState = stateDic[stateType];
        curState.OnEnter();
    }

    public void OnUpdate()
    {
        curState.OnUpdate();
    }
}

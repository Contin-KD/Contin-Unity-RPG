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
    // 当前状态
    public IStateBase curState;
    public Dictionary<StateType, IStateBase> stateDic;
    public FSM()
    {
        this.stateDic = new Dictionary<StateType, IStateBase>();
    }
    /// <summary>
    /// 添加状态
    /// </summary>
    /// <param name="stateType"></param>
    /// <param name="state"></param>
    public void AddState(StateType stateType, IStateBase state)
    {
        if (stateDic.ContainsKey(stateType))
        {
            Debug.Log("状态重复");
            return;
        }
        stateDic.Add(stateType, state);
    }
    /// <summary>
    /// 更改状态
    /// </summary>
    /// <param name="stateType"></param>
    public void SwitchState(StateType stateType)
    {
        if (!stateDic.ContainsKey(stateType))
        {
            Debug.Log("没有该状态");
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

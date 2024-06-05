using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStateBaseSO : ScriptableObject,IState
{

    protected Animator _animator;
    protected List<CharacterStateActionBaseSO> _actions;


    public void InitState(MainStateMachineController machineController)
    {
        _animator = machineController.GetComponentInChildren<Animator>();
        _actions = new List<CharacterStateActionBaseSO>();
    }

    public virtual void OnEnterState()
    {
        
    }

    public abstract void OnStateTick();
    

    public virtual void OnEndState()
    {
       
    }
}

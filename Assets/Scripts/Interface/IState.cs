using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void InitState(MainStateMachineController machineController);
    void OnEnterState();
    void OnStateTick();
    void OnEndState();
}

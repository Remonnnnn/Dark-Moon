using System.Collections;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }

    public bool isAnim;

    public void Intialize(EnemyState _startState)
    {
        currentState= _startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    public bool CheckIsAnim() => isAnim;
}

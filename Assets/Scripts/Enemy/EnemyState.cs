using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    protected bool triggerCalled;
    private string animBoolName;

    protected float stateTimer;

    public EnemyState(Enemy _enemyBase,EnemyStateMachine _enemyStateMachine,string _animBoolName)
    {
        this.enemyBase= _enemyBase;
        this.stateMachine= _enemyStateMachine;
        this.animBoolName= _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer-= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        //enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTriggers()
    {
        triggerCalled = true;
    }
}
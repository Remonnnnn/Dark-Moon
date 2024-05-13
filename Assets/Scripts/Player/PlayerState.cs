using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player,PlayerStateMachine _stateMachine,string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb=player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = InputManager.instance.inputControl.GamePlay.Move.ReadValue<Vector2>().x;
        if(Mathf.Abs(xInput)<.1f)
        {
            xInput = 0;
        }
        yInput = InputManager.instance.inputControl.GamePlay.Move.ReadValue<Vector2>().y;
        if (Mathf.Abs(yInput) < .1f)
        {
            yInput = 0;
        }
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName,false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}

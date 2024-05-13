using System.Collections;
using System.Xml;
using UnityEngine;

public class Enemy_NightBorne : Enemy
{
    #region States

    public NightBorneIdleState idleState { get; private set; }
    public NightBorneBattleState battleState { get; private set; }
    public NightBorneAttackState attackState { get; private set; }
    public NightBorneStunnedState stunnedState { get;private set; }
    public NightBorneDeadState deadState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new NightBorneIdleState(this, stateMachine, "Idle", this);
        battleState = new NightBorneBattleState(this, stateMachine, "Move", this);
        attackState = new NightBorneAttackState(this, stateMachine, "Attack", this);
        stunnedState = new NightBorneStunnedState(this, stateMachine, "Stunned", this);
        deadState = new NightBorneDeadState(this, stateMachine, "Die", this);
    }


    protected override void Start()
    {
        base.Start();
        stateMachine.Intialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();

        GameManager.instance.bossList.Add(enemyName);

        area.GetComponent<AreaBossFightBegun>().ExitBossFight();
        stateMachine.ChangeState(deadState);

    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType { big,medium,small}
public class Enemy_Slime : Enemy
{
    [Header("Slime spesific")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimeToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 maxCreationVelocity;
    [SerializeField] private Vector2 minCreationVelocity;
    #region States

    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);

        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Die", this);

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

        stateMachine.ChangeState(deadState);

        if(slimeType==SlimeType.small)
        {
            return;
        }

        CreateSlime(slimeToCreate, slimePrefab);
    }

    private void CreateSlime(int _amountOfSlime,GameObject _slimePrefab)
    {
        for(int i=0;i<_amountOfSlime;i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<Enemy_Slime>().SetUpSlime(facingDir);//设置自身速度
        }
    }

    public void SetUpSlime(int _facingDir)
    {
        //if(_facingDir!=facingDir)
        //{
        //    Flip();
        //}

        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -_facingDir, yVelocity);

        Invoke("CancelKnockback", 1.5f);
    }

    private void CancelKnockback()=>isKnocked = false;
}

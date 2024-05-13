using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public UI_InGame ui_InGame;

    [Header("Input info")]
    public Vector2 InputDirection;


    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
  
    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get;private set; }

    public SkillManager skill {  get; private set; }
    public GameObject sword { get; private set; }
    public PlayerFX fx { get; private set; }


    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackholeState blackhole { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
   

        fx=GetComponent<PlayerFX>();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump  = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackhole = new PlayerBlackholeState(this, stateMachine, "Jump");

        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();

        InputManager.instance.inputControl.GamePlay.Dash.started += Dash;
        InputManager.instance.inputControl.GamePlay.Crystal.started += UseCrystal;
        InputManager.instance.inputControl.GamePlay.UseFlask.started += UseFlask;

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);


        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

    }

    private void UseFlask_started(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    protected override void Update()
    {
        if(Time.timeScale==0)//暂停时直接返回，拒绝输入
        {
            return;
        }

        InputDirection = InputManager.instance.inputControl.GamePlay.Move.ReadValue<Vector2>();//实读取输入

        base.Update();

        stateMachine.currentState.Update();


    }

    public override void SlowEntityBy(float _slowPercent, float _slowDuration)
    {
        moveSpeed=moveSpeed*(1-_slowPercent);
        jumpForce=jumpForce*(1-_slowPercent);
        dashSpeed=dashSpeed*(1-_slowPercent);
        anim.speed=anim.speed*(1-_slowPercent);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce=defaultJumpForce;
        dashSpeed=defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;


        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }

    #region Input event
    private void Dash(InputAction.CallbackContext obj)
    {
        if (IsWallDetected())
        {
            return;
        }

        if (skill.dash.dashUnlocked == false)
        {
            return;
        }

        if (SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = InputDirection.x;

            ui_InGame.SetCooldownOfDash();

            if (dashDir == 0)
            {
                dashDir = facingDir;
            }

            stateMachine.ChangeState(dashState);
        }
    }

    private void UseCrystal(InputAction.CallbackContext obj)
    {
        if (skill.crystal.crystalUnlocked)
        {
            skill.crystal.CanUseSkill();
            ui_InGame.SetCooldownOfCrystal();
        }
    }

    private void UseFlask(InputAction.CallbackContext obj)
    {
        AudioManager.instance.PlaySFX(41, null);
        if(Inventory.instance.UseFlask())
        {
            fx.PlayRecoverFx();
            ui_InGame.SetCooldownOfFlask();
        }
        
    }

    #endregion

    private void OnDestroy()
    {
        InputManager.instance.inputControl.GamePlay.Dash.started -= Dash;
        InputManager.instance.inputControl.GamePlay.Crystal.started -= UseCrystal;
        InputManager.instance.inputControl.GamePlay.UseFlask.started -= UseFlask;
    }

    public void ReLoadPlayer()
    {
        stats.SetLoadStats();
        stateMachine.ChangeState(idleState);
    }
}

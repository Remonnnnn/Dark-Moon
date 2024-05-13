using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    private bool Succeed = false;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(!Succeed)
        {
            player.SetZeroVelocity();
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Arrow_Controller>()!= null)
            {
                Succeed = true;
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                SuccessfulCounterAttack();

                player.fx.CreateHitFx(hit.transform, true);
                player.fx.ScreenShake(player.fx.shakeSwordImpact);
                player.fx.PlayDustFx();
                rb.velocity = new Vector2(5 * -player.facingDir, rb.velocity.y);


            }

            if (hit.GetComponent<Enemy>() != null)
            {
                if(hit.GetComponent<Enemy>().CanBeStunned())
                {
                    Succeed = true;
                    SuccessfulCounterAttack();

                    hit.GetComponent<CharacterStats>().DecreaseHealthBy(player.stats.damage.GetValue(), Color.red);//反击成功造成一次固定伤害

                    //播放特效
                    InputManager.instance.SetMotorGamepad(.25f, .25f, .75f);
                    player.fx.CreateHitFx(hit.transform, true);
                    player.fx.ScreenShake(player.fx.shakeSwordImpact);
                    player.fx.PlayDustFx();
                    rb.velocity = new Vector2(5 * -player.facingDir, rb.velocity.y);


                    player.skill.parry.RestoreOnParry();//拓展技能 回复

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parry.MakeMirageOnParry(hit.transform);//拓展技能 召唤分身
                    }

                }
            }
        }


        if(stateTimer<0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void SuccessfulCounterAttack()
    {
        stateTimer = 10;//防止过早退出
        player.anim.SetBool("SuccessfulCounterAttack", true);
        AudioManager.instance.PlaySFX(0, null);
    }
}

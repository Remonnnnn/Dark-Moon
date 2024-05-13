using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy.Shady
{
    public class ShadyGroundedState : EnemyState
    {
        protected Transform player;
        protected Enemy_Shady enemy;
        public ShadyGroundedState(global::Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,Enemy_Shady _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
        {
            this.enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();
            player = PlayerManager.instance.player.transform;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.agroDistance)
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}
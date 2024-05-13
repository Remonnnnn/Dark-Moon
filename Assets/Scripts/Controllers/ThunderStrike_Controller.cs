using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    protected PlayerStats playerStats;

    protected void Start()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>()!=null)
        {
            playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

            EnemyStats enemyTarget=collision.GetComponent<EnemyStats>();

            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}

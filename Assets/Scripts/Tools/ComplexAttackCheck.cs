using System.Collections;
using UnityEngine;


public class ComplexAttackCheck : MonoBehaviour
{
    public Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit!");
        if(collision.GetComponent<Player>() != null)
        {
            PlayerStats target = collision.GetComponent<PlayerStats>();
            enemy.stats.DoDamage(target);
        }
    }
}
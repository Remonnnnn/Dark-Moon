using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats=PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.Max(1,Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent));

        playerStats.IncreaseHealthBy(healAmount);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class AreaBossFightBegun : MonoBehaviour
{
    [Header("TimeLineSetting")]
    [SerializeField] private float waitDuration;
    [SerializeField] private PlayableAsset enemyTimeLine;
    [SerializeField] private int bgmIndex;

    public UI_InGame ui_InGame;
    //用于boss战触发
    public Enemy enemy;
    private Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;
        ui_InGame = UI_Manager.instance.UI_Canvas.inGameUI.GetComponent<UI_InGame>();

        enemy.area = gameObject.GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemy!=null && !enemy.bossFightBegun && collision.GetComponent<Player>() != null)
        {
            StartCoroutine(BossFightBegun());
        }
    }

    public void ExitBossFight()
    {
        ui_InGame.ClearEnemyInfo();
        if(!player.stats.isDead)
        {
            AudioManager.instance.TransformBGMTo(SceneLoader.instance.currentLoadedScene.bgmIndex);
        }


        player.stats.DieEvent -= ExitBossFight;
    }

    IEnumerator BossFightBegun()
    {
        InputManager.instance.inputControl.GamePlay.Disable();
        AudioManager.instance.PlayBGM(bgmIndex);

        DirectorManager.instance.SetPlayable(enemyTimeLine,enemy.anim);
        yield return new WaitForSeconds(waitDuration);

        InputManager.instance.inputControl.GamePlay.Enable();
        ui_InGame.SetEnemyInfo(enemy.enemyName, enemy.GetComponent<EnemyStats>());
        AudioManager.instance.CanPlayBGM();
        AudioManager.instance.PlayBGM(enemy.bossBGMId);
        enemy.bossFightBegun = true;
        player.stats.DieEvent += ExitBossFight;
    }

}

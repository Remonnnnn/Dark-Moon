using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    public UI UI_Canvas;
    public Player player;

    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;

    public GameSceneSo firstLoadScene;

    public GameSceneSo currentLoadedScene;
    private GameSceneSo sceneToLoad;

    [Header("黑屏渐入渐出")]
    public bool canFadeScreen;
    public bool unFadeOut;
    public UI_FadeScreen fadeScreen;
    public float fadeDuration;

    private bool isLoading;
    public bool canLoadGame;
    public bool canLoadItem;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
        if (firstLoadScene != null)
        {
            currentLoadedScene = firstLoadScene;
        }

        currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        InputManager.instance.SetUIInput(false);
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void OnLoadRequestEvent(GameSceneSo locationToLoad, bool _canFadeScreen)
    {
        if(isLoading)
        {
            return;
        }

        isLoading = true;
        sceneToLoad = locationToLoad;
        canFadeScreen = _canFadeScreen;

        if (currentLoadedScene != null)
        {
            StartCoroutine(UnloadPreviousScene());
        }
    }

    private IEnumerator UnloadPreviousScene()//卸载当前场景
    {
        if(canFadeScreen && !unFadeOut)
        {
            fadeScreen.FadeOut();
        }

        if(unFadeOut)//说明从死亡场景来的
        {
            UI_Canvas.CloseAllUI();
        }
        unFadeOut = false;

        yield return new WaitForSeconds(fadeDuration);

        yield return currentLoadedScene.sceneReference.UnLoadScene();//卸载当前场景引用 并且等待卸载完成

        player.transform.position = sceneToLoad.startPos;
        player.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void LoadNewScene()//加载新场景
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)//加载完新场景后执行的逻辑
    {

        currentLoadedScene = sceneToLoad;

        GameManager.instance.SetSceneCheckPoints();

        if (canLoadGame)
        {
            SaveManager.instance.LoadGame();
            canLoadGame = false;
        }

        if (sceneToLoad.sceneType==SceneType.location)//如果要加载的不是主菜单
        {
            player.gameObject.SetActive(true);

            if(canLoadItem)
            {
                Inventory.instance.AddLoadItems();
                canLoadItem= false;
            }
            SkillManager.instance.CheckSkill();//使技能检测技能树中的技能状态
            InputManager.instance.SetUIInput(true);
 
            UI_Canvas.SetInGameUI();
        }
        else
        {

            InputManager.instance.SetUIInput(false);
        }

        AudioManager.instance.CanPlayBGM();
        AudioManager.instance.PlayBGM(currentLoadedScene.bgmIndex);

        if (canFadeScreen)
        {
            fadeScreen.FadeIn();
        }

        isLoading= false;
    }

}

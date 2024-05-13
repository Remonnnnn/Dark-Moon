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

    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;

    public GameSceneSo firstLoadScene;

    public GameSceneSo currentLoadedScene;
    private GameSceneSo sceneToLoad;

    [Header("�������뽥��")]
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

    private IEnumerator UnloadPreviousScene()//ж�ص�ǰ����
    {
        if(canFadeScreen && !unFadeOut)
        {
            fadeScreen.FadeOut();
        }

        if(unFadeOut)//˵����������������
        {
            UI_Canvas.CloseAllUI();
        }
        unFadeOut = false;

        yield return new WaitForSeconds(fadeDuration);

        yield return currentLoadedScene.sceneReference.UnLoadScene();//ж�ص�ǰ�������� ���ҵȴ�ж�����

        player.transform.position = sceneToLoad.startPos;
        player.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void LoadNewScene()//�����³���
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)//�������³�����ִ�е��߼�
    {

        currentLoadedScene = sceneToLoad;

        GameManager.instance.SetSceneCheckPoints();

        if (canLoadGame)
        {
            SaveManager.instance.LoadGame();
            canLoadGame = false;
        }

        if (sceneToLoad.sceneType==SceneType.location)//���Ҫ���صĲ������˵�
        {
            player.gameObject.SetActive(true);

            if(canLoadItem)
            {
                Inventory.instance.AddLoadItems();
                canLoadItem= false;
            }
            SkillManager.instance.CheckSkill();//ʹ���ܼ�⼼�����еļ���״̬
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

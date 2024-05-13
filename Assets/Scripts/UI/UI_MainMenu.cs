using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private SceneLoadEventSO loadEventSO;
    public GameSceneSo sceneToGo;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject continueButton;


    private void Start()
    {
        if (!SaveManager.instance.HasSavedData())//检查是否有存档
        {
            continueButton.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(newGameButton);

    }
    public void ContinueGame()
    {
        SkillManager.instance.ReLoadSkill();
        Inventory.instance.ReLoadItem();

        AudioManager.instance.PlaySFX(43, null);
        SceneLoader.instance.canLoadGame = true;//标记此时可加载存档
        SceneLoader.instance.canLoadItem = true;
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, true);
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();

        SkillManager.instance.ReLoadSkill();
        Inventory.instance.ReLoadItem();

        AudioManager.instance.PlaySFX(43, null);
        Inventory.instance.AddStartingItem();
        SceneLoader.instance.canLoadGame = true;
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, true);
    }

    public void ExitGame()
    {
        AudioManager.instance.PlaySFX(43, null);
        Debug.Log("Exit Game");
        Application.Quit();
    }


}


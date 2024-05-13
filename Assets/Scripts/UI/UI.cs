using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UI : MonoBehaviour,ISaveManager
{
    public PlayerInputController inputControl;
    public GameObject FirstButton;

    [Header("End screen")]
    [SerializeField]private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject returnButton;
    [Space]
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    public GameObject inGameUI;

    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow craftWindow;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;


    private void Awake()
    {
        skillTreeUI.SetActive(true);
    }
    private void Start()
    {
        fadeScreen = SceneLoader.instance.fadeScreen;


        inputControl = InputManager.instance.inputControl;
        inputControl.UI.Menu.started += OpenMenu;

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);

        skillTreeUI.SetActive(false);
    }
    

    private void Update()
    {
        if(skillTreeUI.activeInHierarchy)//显示
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<UI_SkillTreeSlot>()?.OnPointerEnter(null);
        }
        else
        {
            skillToolTip.HideToolTip();
        }

        if(characterUI.activeInHierarchy)
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<UI_ItemSlot>()?.OnPointerEnter(null);
            EventSystem.current.currentSelectedGameObject.GetComponent<UI_Equipment>()?.OnPointerEnter(null);
        }
        else
        {
            itemToolTip.HideToolTip();
        }
    }

    public void SwitchTo(GameObject _menu)
    {
        for(int i=1;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu!=null)
        {
            AudioManager.instance.PlaySFX(7, null);
            _menu.SetActive(true);
        }

    }

    //public void SwitchWithKeyTo(GameObject _menu)//用按键进入UIMenu
    //{
    //    if(_menu!=null && _menu.activeSelf)
    //    {
    //        _menu.SetActive(false);
    //        CheckForInGameUI();
    //        return;
    //    }

    //    SwitchTo(_menu);
    //}

    //private void CheckForInGameUI()
    //{
    //    for(int i=0;i<transform.childCount; i++)
    //    {
    //        if(transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>()==null)
    //        {
    //            return;
    //        }
    //    }

    //    SwitchTo(inGameUI);
    //}

    public void SwitchOnEndScreen()
    {
        SwitchTo(null);//关闭inGameUI
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(2);
        AudioManager.instance.StopBGMWithTime();
        AudioManager.instance.PlaySFX(11, null);
        endText.SetActive(true);
        yield return new WaitForSeconds(2);
        restartButton.SetActive(true);
        returnButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartButton);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void ReturnMenuButton() => GameManager.instance.ReturnMenu();

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string,float> pair in _data.volumeSettings)
        {
            foreach(UI_VolumeSlider item in volumeSettings)
            {
                if(item.parametr==pair.Key)
                {
                    item.LoadSlider(pair.Value);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach(UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parametr, item.slider.value);
        }
    }
    #region input event

    public void OpenMenu(InputAction.CallbackContext obj)
    {
        if(!GameManager.instance.isPaused)
        {
            GameManager.instance.PauseGame(true);
            transform.GetChild(0).gameObject.SetActive(true);
            SwitchTo(characterUI);
            Debug.Log(1);
            EventSystem.current.SetSelectedGameObject(FirstButton);
        }
        else
        {
            GameManager.instance.PauseGame(false);
            transform.GetChild(0).gameObject.SetActive(false);
            SwitchTo(inGameUI);
            EventSystem.current.firstSelectedGameObject = null;
        }
    }

    #endregion


    public void CloseAllUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        inputControl.UI.Menu.started -= OpenMenu;
    }

    public void SetInGameUI()
    {
        inGameUI.SetActive(true);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Options : MonoBehaviour
{
    [SerializeField] private UI ui;
    [SerializeField] private SceneLoadEventSO loadEventSO;
    public GameSceneSo sceneToGo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExitGame()
    {
        SaveManager.instance.SaveGame();

        ui.inGameUI.GetComponent<UI_InGame>().ClearEnemyInfo();
        ui.CloseAllUI();

        loadEventSO.RaiseLoadRequestEvent(sceneToGo, true);
        GameManager.instance.PauseGame(false);
    }
}

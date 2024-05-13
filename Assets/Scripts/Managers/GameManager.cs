using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;

    private Player player;
    public bool isPaused;

    [SerializeField] private SceneLoadEventSO loadEventSO;
    [SerializeField] private Checkpoint[] checkpoints;//检查点
    [SerializeField] private string closestCheckpointId;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    [Header("Has KO Boss")]
    public List<string> bossList;
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
    }

    private void Start()
    {
        player=PlayerManager.instance.player;
        bossList = new List<string>();
    }
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        SceneLoader.instance.canLoadGame = true;
        SceneLoader.instance.unFadeOut = true;
        loadEventSO.RaiseLoadRequestEvent(SceneLoader.instance.currentLoadedScene, true);
    }

    public void ReturnMenu()
    {
        SaveManager.instance.SaveGame();
        SceneLoader.instance.unFadeOut = true;
        loadEventSO.RaiseLoadRequestEvent(SceneLoader.instance.firstLoadScene, true);
    }

    public void LoadData(GameData _data)=> StartCoroutine(LoadWithDelay(_data));//加载检查点与死亡点

    private void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                }
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if(lostCurrencyAmount>0)//当有money时，留下尸体
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency= lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private void LoadKOBoss(GameData _data)
    {
        bossList = _data.hasKOBoss;

        foreach(var name in bossList)
        {
            Destroy(GameObject.Find("Enemy_" + name));
        }
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadCheckpoints(_data);
        PlacePlayerAtClosestCheckpoint(_data);
        LoadLostCurrency(_data);
        LoadKOBoss(_data);
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount= lostCurrencyAmount;
        _data.lostCurrencyX=player.transform.position.x;
        _data.lostCurrencyY=player.transform.position.y;

        if(FindClosestCheckpoint()!=null)
        {
            _data.closestCheckpointId=FindClosestCheckpoint().id;
        }

        _data.hasKOBoss = bossList;

        _data.checkpoints.Clear();

        foreach(Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }

    }

    private void PlacePlayerAtClosestCheckpoint(GameData _data)
    {
        //if (_data.closestCheckpointId == null)
        //{
        //    return;
        //}

        PlayerManager.instance.player.ReLoadPlayer();//重置Player状态
        closestCheckpointId = _data.closestCheckpointId;//找到存档中死亡信息

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closestCheckpointId == checkpoint.id)
            {
                player.transform.position = checkpoint.transform.position;
                return;
            }
        }

        //在该scene下没有存档信息
        player.transform.position = SceneLoader.instance.currentLoadedScene.startPos;
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach(var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.transform.position, checkpoint.transform.position);

            if(distanceToCheckpoint<closestDistance && checkpoint.activationStatus==true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if(_pause)
        {
            Time.timeScale = 0;
            InputManager.instance.inputControl.GamePlay.Disable();
            isPaused=true;
        }
        else
        {
            Time.timeScale = 1;
            InputManager.instance.inputControl.GamePlay.Enable();
            isPaused =false;
        }
    }

    public void SetSceneCheckPoints()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    private float blackholeTimer;

    private bool canShrink;
    private bool canGrow = true;
    private bool canCreateHotKey = true;
    private bool cloneAttackReleased;
    private bool playerCanDisappear = true;

    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    public List<Transform> targets=new List<Transform>();
    private List<GameObject>createdHotKey=new List<GameObject>();

    public bool playerCanExitState { get; private set; }

    public void SetupBlackhole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _amountOfAttacks,float _cloneAttackCooldown,float _blackholeDuration)
    {
        InputManager.instance.inputControl.GamePlay.Blackhole.started += ReleaseCloneAttackBySelf;

        maxSize = _maxSize;
        growSpeed= _growSpeed;
        shrinkSpeed= _shrinkSpeed;
        amountOfAttacks= _amountOfAttacks;
        cloneAttackCooldown= _cloneAttackCooldown;

        blackholeTimer= _blackholeDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
        {
            playerCanDisappear = false;
        }
    }
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if(blackholeTimer<0)
        {
            blackholeTimer = Mathf.Infinity;

            if(targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAbility();
            }
        }
        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                canShrink = false;
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttackBySelf(InputAction.CallbackContext callbackContext)
    {
        ReleaseCloneAttack();
    }
    private void ReleaseCloneAttack()
    {
        if(targets.Count<=0)
        {
            return;
        }

        //DestroyHotKey();
        cloneAttackReleased = true;
        canCreateHotKey = false;

        if(playerCanDisappear)
        {
            playerCanDisappear= false;
            PlayerManager.instance.player.fx.MakeTransprent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks>0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
            {
                xOffset = 1;
            }
            else
            {
                xOffset = -1;
            }

            if(SkillManager.instance.clone.crystalInsteadOfClone)//当分身被替换为水晶时
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            }

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility",.5f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        //DestroyHotKey();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void DestroyHotKey()
    {
        if(createdHotKey.Count<=0)
        {
            return;
        }
        for(int i=0;i<createdHotKey.Count;i++)
        {
            Destroy(createdHotKey[i]);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>()!=null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);//静止时间

            //CreateHotKey(collision);
            AddEnemyToList(collision.GetComponent<Enemy>().transform);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent <Enemy>()!=null)
        {
            collision.GetComponent <Enemy>().FreezeTime(false);
        }
    }

    //private void CreateHotKey(Collider2D collision)
    //{
    //    if(keyCodeList.Count < 0)
    //    {
    //        Debug.Log("Not enough");
    //        return;
    //    }
        
    //    if(!canCreateHotKey)
    //    {
    //        return;
    //    }

    //    GameObject newHotkey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

    //    KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
    //    createdHotKey.Add(newHotkey);
    //    keyCodeList.Remove(choosenKey);

    //    Blackhole_HotKey_Controller newHotKeyScropt = newHotkey.GetComponent<Blackhole_HotKey_Controller>();

    //    newHotKeyScropt.SetupHotKey(choosenKey, collision.transform, this);
    //}

    public void AddEnemyToList(Transform _enemyTransform)=>targets.Add(_enemyTransform);
}

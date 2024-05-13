using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadButton;
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Crystal simple")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked;

    [Header("Explode crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;
    [SerializeField] private float explodeRadius;

    [Header("Move crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingButton;
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField]private UI_SkillTreeSlot unlockMultiStackButton;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField]private int multiCnt;
    private float multiStackTimer;


    protected override void Start()
    {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
    }

    protected override void Update()
    {
        base.Update();

        if (canUseMultiStacks)//回复水晶
        {
            multiStackTimer -= Time.deltaTime;

            if (multiCnt < amountOfStacks && multiStackTimer < 0)//当水晶未满时回复
            {
                RefilCrystal();
                multiStackTimer = multiStackCooldown;
            }

        }

    }

    //技能解锁区域
    #region Unlock skill region

    public override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
        UnlockMultiStack();

    }
    private void UnlockCrystal()
    {
        if(unlockCrystalButton.unlocked)
        {
            crystalUnlocked = true;
            UI_inGame_Image.SetActive(true);
        }
    }

    private void UnlockCrystalMirage()
    {
        if(unlockCloneInsteadButton.unlocked)
        {
            cloneInsteadOfCrystal= true;
        }
    }

    private void UnlockExplosiveCrystal()
    {
        if(unlockExplosiveButton.unlocked)
        {
            canExplode= true;
        }
    }

    private void UnlockMovingCrystal()
    {
        if(unlockMovingButton.unlocked)
        {
            canMove = true;
        }
    }

    private void UnlockMultiStack()
    {
        if (unlockMultiStackButton.unlocked)
        {
            canUseMultiStacks = true;
        }
    }

    public override void InitUnlock()
    {
        crystalUnlocked = false;
        cloneInsteadOfCrystal = false;
        canExplode= false;
        canMove= false;
        canUseMultiStacks= false;

        UI_inGame_Image.SetActive(false);

        unlockCrystalButton.InitSkiil();
        unlockCloneInsteadButton.InitSkiil();
        unlockExplosiveButton.InitSkiil();
        unlockMovingButton.InitSkiil();
        unlockMultiStackButton.InitSkiil();
    }
    #endregion
    public override void UseSkill()
    {
        base.UseSkill();

        if(canUseMultiStacks)//可以堆叠水晶时
        {
            CanUseMultiCrystal();
            return;
        }

        if(currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if(canMove)//当启用水晶自动寻找时，传送技能不可用
            {
                return;
            }

            Vector2 playerPos=player.transform.position;
            player.transform.position=currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(cloneInsteadOfCrystal)//当解锁分身替换水晶时调用
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, explodeRadius, FindClosetEnemty(currentCrystal.transform),player);
    }

    public void CurrentCrystalChooseRandomTarget()=>currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private void CanUseMultiCrystal()
    {
        if (multiCnt > 0)
        {
            GameObject newCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);

            newCrystal.GetComponent<Crystal_Skill_Controller>().
                SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, explodeRadius, FindClosetEnemty(newCrystal.transform), player);

            multiCnt--;
        }
    }
    private void RefilCrystal()//回复一个水晶
    {
        multiCnt++;
    }
}

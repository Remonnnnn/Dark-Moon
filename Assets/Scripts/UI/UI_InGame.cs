using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider playerSlider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;

    [Header("Money info")]
    [SerializeField] private TextMeshProUGUI currentMoney;
    [SerializeField] private float moneyAmount;
    [SerializeField] private float increaseRate = 100;

    [Header("Boss info")]
    public TextMeshProUGUI enemyName;
    public EnemyStats enemyStats;
    public Slider enemySlider;

    void Start()
    {
        if(playerStats!=null)
        {
            playerStats.onHealthChanged += UpdatePlayerHealthUI;
        }

        skills = SkillManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoneyUI();

        //if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
        //{
        //    SetCooldownOf(crystalImage);
        //}
        //if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
        //{
        //    SetCooldownOf(swordImage);
        //}
        //if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
        //{
        //    SetCooldownOf(blackholeImage);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
        //{
        //    SetCooldownOf(flaskImage);
        //}

        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        CheckCooldownOf(swordImage, skills.sword.cooldown);
        CheckCooldownOf(blackholeImage, skills.blackhole.cooldown);

        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);

    }

    private void UpdateMoneyUI()
    {
        if (moneyAmount < PlayerManager.instance.GetCurrency())
        {
            moneyAmount += Time.deltaTime * increaseRate;
        }
        else
        {
            moneyAmount = PlayerManager.instance.GetCurrency();
        }

        currentMoney.text = ((int)moneyAmount).ToString();
    }

    private void UpdatePlayerHealthUI()
    {
        playerSlider.maxValue = playerStats.GetMaxHealthValue();
        playerSlider.value = playerStats.currentHealth;
    }

    private void UpdateEnemyHealthUI()
    {
        enemySlider.maxValue = enemyStats.GetMaxHealthValue();
        enemySlider.value = enemyStats.currentHealth;
    }

    private void CheckCooldownOf(Image _image,float _cooldown)
    {
        if(_image.fillAmount>0)
        {
            _image.fillAmount-=1/_cooldown*Time.deltaTime;
        }
    }

    public void SetEnemyInfo(string _enemyName,EnemyStats _enemyStats)
    {
        enemyName.text = _enemyName;
        enemyStats= _enemyStats;
        enemyStats.onHealthChanged += UpdateEnemyHealthUI;
        enemySlider.gameObject.SetActive(true);
    }

    public void ClearEnemyInfo()
    {
        enemySlider.gameObject.SetActive(false);
        enemySlider.maxValue = 1;
        enemySlider.value = 1;
        enemyStats = null;
        enemyName.text = "";
    }

    #region SetCooldownImage

    public void SetCooldownOfDash()
    {
        if (dashImage.fillAmount <= 0)
        {
            dashImage.fillAmount = 1;
        }
    }

    public void SetCooldownOfParry()
    {
        if (parryImage.fillAmount <= 0)
        {
            parryImage.fillAmount = 1;
        }
    }

    public void SetCooldownOfCrystal()
    {
        if (crystalImage.fillAmount <= 0)
        {
            crystalImage.fillAmount = 1;
        }
    }

    public void SetCooldownOfFlask()
    {
        if (flaskImage.fillAmount <= 0)
        {
            flaskImage.fillAmount = 1;
        }
    }
    
    public void SetCooldownOfBlackhole()
    {
        if(blackholeImage.fillAmount <= 0)
        {
            blackholeImage.fillAmount = 1;
        }
    }

    #endregion



}

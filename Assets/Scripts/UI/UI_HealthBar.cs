using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private CharacterStats myStats => GetComponentInParent<CharacterStats>();
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        UpdateHealthUI();

        gameObject.SetActive(false);//受击后再显示血量条
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void SetHealthUIVisual()
    {
        gameObject.SetActive(true);
        UpdateHealthUI();
        myStats.onHealthChanged -= SetHealthUIVisual;
    }

    private void OnEnable()
    {
        myStats.onHealthChanged += SetHealthUIVisual;
        myStats.onHealthChanged += UpdateHealthUI;
        entity.onFlipped += FlipUI;
    }
    private void FlipUI()=> myTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        if(entity!=null)
        {
            entity.onFlipped -= FlipUI;
            myStats.onHealthChanged -= UpdateHealthUI;
        }
    }
}

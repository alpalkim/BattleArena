using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BattleUnit))]
public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text HPText;
    [SerializeField] private Image unitImage;
    [SerializeField] private Slider hpSlider;

    private BattleUnit _battleUnit;

    private void Start()
    {
        _battleUnit = GetComponent<BattleUnit>();

        nameText.text = _battleUnit.unitName + "\n" + "(Lvl." + _battleUnit.level + ")";
        damageText.text = "Damage: " + _battleUnit.damage;
        HPText.text = "HP: " + _battleUnit.currentHP + "/" + _battleUnit.initialHP;

        hpSlider.value = _battleUnit.currentHP;
        hpSlider.maxValue = _battleUnit.initialHP;

        unitImage.sprite = _battleUnit.unitSprite;
    }

    public void UpdateUI()
    {
        HPText.text = "HP: " + _battleUnit.currentHP + "/" + _battleUnit.initialHP;
        hpSlider.value = _battleUnit.currentHP;
    }
}
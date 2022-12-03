using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BattleUnit))]
public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text damageText;
    [SerializeField] private Image unitImage;
    [SerializeField] private Slider hpSlider;

    private BattleUnit _battleUnit;

    private void Start()
    {
        _battleUnit = GetComponent<BattleUnit>();

        nameText.text = _battleUnit.unitName + "\n" + "(Lvl."+_battleUnit.level + ")";
        damageText.text = "Damage: "+_battleUnit.damage.ToString();
        hpSlider.maxValue = _battleUnit.initialHP;
        hpSlider.value = _battleUnit.currentHP;
        unitImage.sprite = _battleUnit.unitSprite;
    }

    public void UpdateHP()
    {
        hpSlider.value = _battleUnit.currentHP;
    }
}
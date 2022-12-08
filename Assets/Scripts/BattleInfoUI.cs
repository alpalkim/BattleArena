using UnityEngine;
using UnityEngine.UI;

public class BattleInfoUI : MonoBehaviour
{
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _attackPowerText;
    [SerializeField] private Text _HPText;
    [SerializeField] private Slider _hpSlider;

    private BattleUnit _battleUnit;
    public void Init(BattleUnit battleUnit)
    {
        _battleUnit = battleUnit;

        _nameText.text = _battleUnit.BattleUnitObject.name;
        _attackPowerText.text = "Damage: " + _battleUnit.BattleUnitObject.GetAttackPower();
        _HPText.text = "HP: " + _battleUnit.CurrentHP + "/" + _battleUnit.BattleUnitObject.GetHP();
        _hpSlider.maxValue = _battleUnit.BattleUnitObject.GetHP();
        _hpSlider.value = _battleUnit.CurrentHP;
    }

    public void UpdateUI()
    {
        _HPText.text = "HP: " + _battleUnit.CurrentHP + "/" + _battleUnit.BattleUnitObject.GetHP();
        _hpSlider.value = _battleUnit.CurrentHP;
    }
}
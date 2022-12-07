using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _damageText;
    [SerializeField] private Text _hpText;
    [SerializeField] private Text _experienceText;
    [SerializeField] private Text _levelText;

    public void Init(BattleUnitSO battleUnitObject)
    {
        _nameText.text = battleUnitObject.name;
        _damageText.text = battleUnitObject.GetAttackPower().ToString();
        _hpText.text = battleUnitObject.GetHP().ToString();
        _experienceText.text = battleUnitObject.GetExperiencePoint().ToString();
        _levelText.text = battleUnitObject.GetLevel().ToString();
    }
}
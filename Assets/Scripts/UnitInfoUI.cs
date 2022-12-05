using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text HPText;
    [SerializeField] private Text Experience;
    [SerializeField] private Text Level;

    public void Init(BattleUnitSO battleUnitObject)
    {
        nameText.text = battleUnitObject.name;
        damageText.text = battleUnitObject.AttackPower.ToString();
        HPText.text = battleUnitObject.InitialHP.ToString();
        Experience.text = battleUnitObject.ExperiencePoint.ToString();
        Level.text = battleUnitObject.Level.ToString();
    }
}
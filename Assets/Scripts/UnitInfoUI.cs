using UnityEngine;
using UnityEngine.UI;

// [RequireComponent(typeof(BattleUnit))]
public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text HPText;
    [SerializeField] private Text Experience;
    [SerializeField] private Text Level;
    // [SerializeField] private Image unitImage;
    // [SerializeField] private Slider hpSlider;
    
    // private void Start()
    // {
    //     _battleUnit = GetComponent<BattleUnit>();
    //
    //     nameText.text = _battleUnit.UnitName + "\n" + "(Lvl." + _battleUnit.level + ")";
    //     damageText.text = "Damage: " + _battleUnit.Damage;
    //     HPText.text = "HP: " + _battleUnit.CurrentHP + "/" + _battleUnit.InitialHP;
    //     hpSlider.value = _battleUnit.CurrentHP;
    //     hpSlider.maxValue = _battleUnit.InitialHP;
    // }

    public void Init(BattleUnitSO battleUnitObject)
    {
        // unitImage.sprite = battleUnitObject.UnitSprite;
        nameText.text = battleUnitObject.name;
        damageText.text = battleUnitObject.AttackPower.ToString();
        HPText.text = battleUnitObject.InitialHP.ToString();
        Experience.text = battleUnitObject.ExperiencePoint.ToString();
        Level.text = battleUnitObject.Level.ToString();
    }

    public void UpdateUI()
    {
        // HPText.text = "HP: " + _battleUnit.CurrentHP + "/" + _battleUnit.InitialHP;
        // hpSlider.value = _battleUnit.CurrentHP;
    }
}
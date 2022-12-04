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

        nameText.text = _battleUnit.UnitName + "\n" + "(Lvl." + _battleUnit.level + ")";
        damageText.text = "Damage: " + _battleUnit.Damage;
        HPText.text = "HP: " + _battleUnit.CurrentHP + "/" + _battleUnit.InitialHP;

        hpSlider.value = _battleUnit.CurrentHP;
        hpSlider.maxValue = _battleUnit.InitialHP;

        unitImage.sprite = _battleUnit.UnitSprite;
    }

    public void UpdateUI()
    {
        HPText.text = "HP: " + _battleUnit.CurrentHP + "/" + _battleUnit.InitialHP;
        hpSlider.value = _battleUnit.CurrentHP;
    }
}
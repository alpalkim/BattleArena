using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattleUnit : MonoBehaviour, IAttack, ITakeDamage
{
    protected BattleManager _battleManager;
    protected HeroSelectionMenuController _heroSelectionController;

    [SerializeField] private GameObject _deadPanel;
    [SerializeField] private BattleInfoUI _battleInfoUI;
    [SerializeField] private Image _unitImage;

    public BattleUnitSO BattleUnitObject;
    public int CurrentHP { get; private set; }

    protected WaitForSeconds _waitForAttackAnimation;
    protected float _attackAnimationDuration = GlobalSettings.AttackAnimationDuration;
    protected IEnumerator _heroAttackAnimationCoroutine;
    protected IEnumerator _bossAnimationCoroutine;

    // To be initialized from hero selection scene
    public virtual void Init(HeroSelectionMenuController heroSelectionController, BattleUnitSO battleUnitObject)
    {
        _heroSelectionController = heroSelectionController;
        BattleUnitObject = battleUnitObject;
        _unitImage.sprite = battleUnitObject.GetUnitSprite();
    }

    // To be initialized from battle scene 
    public virtual void Init(BattleManager battleManager, BattleUnitSO battleUnitObject)
    {
        BattleUnitObject = battleUnitObject;
        _unitImage.sprite = battleUnitObject.GetUnitSprite();
        _battleManager = battleManager;
        CurrentHP = battleUnitObject.GetHP();
        _waitForAttackAnimation = new WaitForSeconds(_attackAnimationDuration);
        _battleInfoUI.Init(this);
    }

    public abstract void Attack();

    public void TakeDamage(int damageAmount)    //Generic method of taking damage for both hero and boss
    {
        CurrentHP -= damageAmount;
        if (CurrentHP <= 0) Die();
        _battleInfoUI.UpdateUI();
    }

    public virtual void Die()
    {
        CurrentHP = 0;
        _deadPanel.SetActive(true);
    }
}
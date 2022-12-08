using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattleUnit : MonoBehaviour, IAttack, ITakeDamage
{
    protected BattleManager _battleManager;
    protected HeroSelectionMenuController _heroSelectionController;

    [SerializeField] private GameObject _deadPanel;
    [SerializeField] private BattleInfoUI _battleInfoUI;

    public BattleUnitSO BattleUnitObject;

    [SerializeField] private Image unitImage;

    public int CurrentHP { get; private set; }

    protected WaitForSeconds _waitForAttackAnimation;
    protected float _attackAnimationDuration = GlobalSettings.AttackAnimationDuration;
    protected IEnumerator _heroAttackAnimationCoroutine;
    protected IEnumerator _bossAnimationCoroutine;

    public virtual void Init(BattleManager battleManager, BattleUnitSO battleUnitObject)
    {
        _battleManager = battleManager;
        BattleUnitObject = battleUnitObject;
        unitImage.sprite = battleUnitObject.GetUnitSprite();
        CurrentHP = battleUnitObject.GetHP();
        _waitForAttackAnimation = new WaitForSeconds(_attackAnimationDuration);
        _battleInfoUI.Init(this);
    }

    public virtual void Init(HeroSelectionMenuController heroSelectionController, BattleUnitSO battleUnitObject)
    {
        _heroSelectionController = heroSelectionController;
        BattleUnitObject = battleUnitObject;
        unitImage.sprite = battleUnitObject.GetUnitSprite();
    }

    public abstract void Attack();

    public void TakeDamage(int damageAmount)
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
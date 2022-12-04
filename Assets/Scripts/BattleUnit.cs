using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UnitInfoUI))]
public abstract class BattleUnit : MonoBehaviour, IAttack, ITakeDamage
{
    protected BattleManager _battleManager;
    
    
    [SerializeField] private GameObject _deadPanel;
    private UnitInfoUI _infoUI;

    public Sprite UnitSprite;
    public string UnitName;
    public int Damage;
    public int InitialHP;
    
    private int _currentHP;
    public int CurrentHP
    {
        get { return _currentHP; }
        set { _currentHP = value; }
    }
    public int level;

    protected WaitForSeconds _waitForAttackAnimation;
    protected float _attackAnimationDuration = 2f;
    protected IEnumerator _heroAttackAnimationCoroutine;
    protected IEnumerator _bossAnimationCoroutine;

    public virtual void Init(BattleManager battleManager)
    {
        _waitForAttackAnimation = new WaitForSeconds(_attackAnimationDuration);
        _battleManager = battleManager;
        _infoUI = GetComponent<UnitInfoUI>();
        CurrentHP = InitialHP;
    }

    public abstract void Attack();

    public void TakeDamage(int damageAmount)
    {
        CurrentHP -= damageAmount;
        if (CurrentHP <= 0) Die();
        _infoUI.UpdateUI();
    }

    public virtual void Die()
    {
        CurrentHP = 0;
        _deadPanel.SetActive(true);
        // Die Animation
    }

    protected virtual void IncreaseLevel() => level++;
}
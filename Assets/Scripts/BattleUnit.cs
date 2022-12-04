using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UnitInfoUI))]
public abstract class BattleUnit : MonoBehaviour, IAttack, ITakeDamage
{
    protected BattleManager _battleManager;

    private UnitInfoUI _infoUI;

    public Sprite unitSprite;
    public string unitName;
    public int damage;
    public int initialHP;
    public int currentHP;
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
    }

    public abstract void Attack();

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        if (currentHP <= 0) Die();
        _infoUI.UpdateUI();
    }

    public virtual void Die()
    {
        currentHP = 0;
        // Die Animation
    }

    protected virtual void IncreaseLevel() => level++;
}
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Hero : BattleUnit
{
    private int _experience;

    public override void Init(BattleManager battleManager)
    {
        base.Init(battleManager);
        GetComponent<Button>().onClick.AddListener(Attack);
        _battleManager.onBattleWon += OnBattleWon;
    }
    public override void Attack()
    {
        if (!_battleManager.IsPlayerTurn())
        {
            Debug.Log("Wait for your turn!");
            return;
        }

        if (currentHP <= 0)
        {
            Debug.Log("Dead heroes cannot attack");
            return;
        }

        Debug.Log("Player is attacking!");
        _heroAttackAnimationCoroutine = AttackAnimation();
        if (_heroAttackAnimationCoroutine != null) StopCoroutine(_heroAttackAnimationCoroutine);
        StartCoroutine(_heroAttackAnimationCoroutine);
    }

    private IEnumerator AttackAnimation()
    {
        _battleManager.OnFightStarted(); //To prevent double click on player turn.

        Vector3 initialPosition = transform.position;

        transform.DOMove(_battleManager.GetBossLocation().position, _attackAnimationDuration*0.5f).SetEase(Ease.OutExpo).OnComplete(
            () => { transform.DOMove(initialPosition, _attackAnimationDuration *0.4f).SetEase(Ease.InCirc); }
        );
        yield return _waitForAttackAnimation;
        _battleManager.DamageBoss(damage);
        StopCoroutine(_heroAttackAnimationCoroutine);
    }

    private void OnBattleWon()
    {
        if (currentHP > 0) IncreaseXP();
    }

    private void IncreaseXP()
    {
        _experience++;
        if (_experience % 5 == 0) IncreaseLevel(); // The hero's level increases in every 5 experience points.
    }

    protected override void IncreaseLevel()
    {
        base.IncreaseLevel();
        GetBonusForLevellingUp();
    }

    // The hero gets bonus for HP and damage whenever levels up
    private void GetBonusForLevellingUp()
    {
        initialHP *= (int) 1.1f;
        damage *= (int) 1.1f;
    }

    public override void Die()
    {
        base.Die();
        _battleManager.RemoveHeroFromAliveList(this);
    }
}
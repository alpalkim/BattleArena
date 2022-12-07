using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Enemy : BattleUnit
{
    private BattleUnit _randomHeroToAttack;
    private readonly WaitForSeconds _waitForSeconds = new(1);
    public override void Battle()
    {
        _bossAnimationCoroutine = AttackAnimation();
        if (_bossAnimationCoroutine != null) StopCoroutine(_bossAnimationCoroutine);
        StartCoroutine(_bossAnimationCoroutine);
    }


    private IEnumerator AttackAnimation()
    {
        yield return _waitForSeconds;
        _battleManager.OnFightStarted();
        
        Vector3 initialPosition = transform.position;
        _randomHeroToAttack = _battleManager.GetRandomAliveHero();
        transform.DOMove(_randomHeroToAttack.transform.position, _attackAnimationDuration*0.6f).SetEase(Ease.OutExpo).OnComplete(
            () => { transform.DOMove(initialPosition, _attackAnimationDuration*0.4f).SetEase(Ease.InCirc); }
        );
        yield return _waitForAttackAnimation;
        _battleManager.DamageHero(_randomHeroToAttack,BattleUnitObject.AttackPower);
    }

    public override void Die()
    {
        base.Die();
        BattleUnitObject.Level++; //The boss' level will increase each time it dies
    }
}
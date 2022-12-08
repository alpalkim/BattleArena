using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : BattleUnit
{
    private BattleUnit _randomHeroToAttack;
    private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(1);
    public override void Attack()
    {
        _bossAnimationCoroutine = AttackAnimation();
        if (_bossAnimationCoroutine != null) StopCoroutine(_bossAnimationCoroutine);
        StartCoroutine(_bossAnimationCoroutine);
    }
    
    private IEnumerator AttackAnimation()   // Attack animation for boss
    {
        yield return _waitForSeconds;
        _battleManager.OnFightStarted();
        
        Vector3 initialPosition = transform.position;
        _randomHeroToAttack = _battleManager.GetRandomAliveHero();
        transform.DOMove(_randomHeroToAttack.transform.position, _attackAnimationDuration*0.6f).SetEase(Ease.OutExpo).OnComplete(
            () => { transform.DOMove(initialPosition, _attackAnimationDuration*0.4f).SetEase(Ease.InCirc); }
        );
        yield return _waitForAttackAnimation;
        _battleManager.DamageHero(_randomHeroToAttack,BattleUnitObject.GetAttackPower());
    }
    
    
}
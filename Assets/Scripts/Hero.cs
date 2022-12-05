using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Hero : BattleUnit
{
    [SerializeField] private GameObject _selectionBorder;
    [SerializeField] private GameObject _selectionForeground;
    [SerializeField] private GameObject _lockedPanel;

    public override void Init(BattleManager battleManager, BattleUnitSO battleUnitObject)
    {
        base.Init(battleManager, battleUnitObject);
        GetComponent<Button>().onClick.AddListener(Attack);
        _battleManager.onBattleWon += OnBattleWon;
    }

    public override void Init(HeroSelectionMenuController heroSelectionController, BattleUnitSO battleUnitObject)
    {
        base.Init(heroSelectionController, battleUnitObject);
        GetComponent<Button>().onClick.AddListener(OnHeroClick);
        _lockedPanel.SetActive(BattleUnitObject.IsLocked);
    }

    public override void Attack()
    {
        if (!_battleManager.IsPlayerTurn())
        {
            Debug.Log("Wait for your turn!");
            return;
        }

        if (CurrentHP <= 0)
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

        transform.DOMove(_battleManager.GetBossLocation().position, _attackAnimationDuration * 0.5f).SetEase(Ease.OutExpo).OnComplete(
            () => { transform.DOMove(initialPosition, _attackAnimationDuration * 0.4f).SetEase(Ease.InCirc); }
        );
        yield return _waitForAttackAnimation;
        _battleManager.DamageBoss(BattleUnitObject.AttackPower);
        StopCoroutine(_heroAttackAnimationCoroutine);
    }

    private void OnBattleWon()
    {
        if (CurrentHP > 0) IncreaseXP();
    }

    private void IncreaseXP()
    {
        BattleUnitObject.ExperiencePoint++;
        if (BattleUnitObject.ExperiencePoint % 5 == 0) IncreaseLevel(); // The hero's level increases in every 5 experience points.
    }

    protected override void IncreaseLevel()
    {
        base.IncreaseLevel();
        GetBonusForLevellingUp();
    }

    // The hero gets bonus for HP and damage whenever levels up
    private void GetBonusForLevellingUp()
    {
        BattleUnitObject.InitialHP = (int) (BattleUnitObject.InitialHP * 1.1f);
        BattleUnitObject.AttackPower = (int) (BattleUnitObject.AttackPower * 1.1f);
    }

    public override void Die()
    {
        base.Die();
        _battleManager.RemoveHeroFromAliveList(this);
    }

    private void ShowInfoPopUp()
    {
        _heroSelectionController.ShowInfoPopUp(this);
        GetComponent<Button>().onClick.AddListener(CloseInfoPopUp);
    }

    private void CloseInfoPopUp()
    {
        _heroSelectionController.CloseInfoPopUp();
        GetComponent<Button>().onClick.AddListener(ShowInfoPopUp);
    }

    private void OnHeroClick()
    {
        if (BattleUnitObject.IsLocked)
            return;

        if (BattleUnitObject.IsSelected)
        {
            ToggleSelectionUI();
            _heroSelectionController.DeselectHero(this);
            BattleUnitObject.IsSelected = false;
        }
        else if (_heroSelectionController.CanSelectHero())
        {
            _heroSelectionController.SelectHero(this);
            ToggleSelectionUI();
            BattleUnitObject.IsSelected = true;
        }
    }

    private void ToggleSelectionUI()
    {
        _selectionBorder.SetActive(!_selectionBorder.activeSelf);
        _selectionForeground.SetActive(!_selectionForeground.activeSelf);
    }
}
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Hero : BattleUnit,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private GameObject _selectionBorder;
    [SerializeField] private GameObject _selectionForeground;
    [SerializeField] private GameObject _lockedPanel;

    private Button _myButton;
    
    [SerializeField]  private GameObject _infoPopUp;
    [SerializeField] private UnitInfoUI _infoUI;
    
    
    delegate void MyHeroAction(); 
    MyHeroAction _actionOnClick;

    public override void Init(BattleManager battleManager, BattleUnitSO battleUnitObject)
    {
        base.Init(battleManager, battleUnitObject);
        _actionOnClick = Battle;
        _battleManager.onBattleWon += OnBattleWon;
        _infoUI = _infoPopUp.GetComponent<UnitInfoUI>();
    }

    public override void Init(HeroSelectionMenuController heroSelectionController, BattleUnitSO battleUnitObject)
    {
        base.Init(heroSelectionController, battleUnitObject);
        _actionOnClick = HeroSelection;
        _lockedPanel.SetActive(BattleUnitObject.IsUnitLocked());
    }
    
    private void OnHeroClick()
    {
        _actionOnClick();
    }
    
    private void HeroSelection()
    {
        if (BattleUnitObject.IsUnitLocked())
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

    public override void Battle()
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
        _battleManager.DamageBoss(BattleUnitObject.GetAttackPower());
        StopCoroutine(_heroAttackAnimationCoroutine);
    }

    private void OnBattleWon()
    {
        if (CurrentHP > 0) IncreaseXP();
    }

    private void IncreaseXP()
    {
        BattleUnitObject.IncreaseExperiencePoint();
        if (BattleUnitObject.GetExperiencePoint() % GlobalSettings.RequiredXPAmountToIncreaseLevel == 0) IncreaseLevel(); // The hero's level increases in every 5 experience points.
    }

    private void IncreaseLevel()
    {
        BattleUnitObject.IncreaseLevel();
        GetBonusForLevellingUp();
    }

    // The hero gets bonus for HP and damage whenever levels up
    private void GetBonusForLevellingUp()
    {
        BattleUnitObject.IncreaseAttackPower(GlobalSettings.AttackPowerMultiplierForLevelingUp);
        BattleUnitObject.IncreaseHP(GlobalSettings.HPMultiplierForLevelingUp);
    }

    public override void Die()
    {
        base.Die();
        _battleManager.RemoveHeroFromAliveList(this);
    }

    public void ShowInfoPopUp()
    {
        _infoUI.Init(BattleUnitObject);
        _infoPopUp.SetActive(true);
    }

    private void CloseInfoPopUp()
    {
        _infoPopUp.SetActive(false);
    }

    

    private void ToggleSelectionUI()
    {
        _selectionBorder.SetActive(!_selectionBorder.activeSelf);
        _selectionForeground.SetActive(!_selectionForeground.activeSelf);
    }
    
    private bool _isHoldCompleted;
    private float _timer;
    private Button _button;
    
    private Coroutine _holdRoutine;
    private float _tapAndHoldDuration = GlobalSettings.TapAndHoldDuration;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _holdRoutine = StartCoroutine(nameof(HoldTimerRoutine));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isHoldCompleted) OnHeroClick();
        Stop();
    }
    
    private IEnumerator HoldTimerRoutine()
    {
        _timer = 0;
        _isHoldCompleted = false;
        while (true)
        {
            _timer += Time.deltaTime;
            if (_timer > _tapAndHoldDuration)
            {
                _isHoldCompleted = true;
                ShowInfoPopUp();
                yield break;
            }
            yield return null;
        }
    }

    private void Stop()
    {
        if(_holdRoutine != null) StopCoroutine(_holdRoutine);
        _isHoldCompleted = false;
        CloseInfoPopUp();
    }

}
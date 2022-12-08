using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Hero : BattleUnit, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject _selectionBorder;
    [SerializeField] private GameObject _selectionForeground;
    [SerializeField] private GameObject _lockedPanel;
    
    [SerializeField] private GameObject _infoPopUp;
    [SerializeField] private UnitInfoUI _infoUI;
    [SerializeField] private Text _animatedText;

    private HeroUnitSO _heroAttributes;
    
    delegate void HeroAction();
    HeroAction _heroActionOnClick;    // Since we are using same prefab on both screen, their action on click will change accordingly.

    public override void Init(BattleManager battleManager, BattleUnitSO battleUnitObject)
    {
        _heroAttributes = (HeroUnitSO)battleUnitObject;
        base.Init(battleManager, battleUnitObject);
        _heroActionOnClick = Attack;
        _battleManager.onBattleWon += OnBattleWon;  // Heroes that is on battle will be invoked if the battle is won 
        _infoUI = _infoPopUp.GetComponent<UnitInfoUI>();
    }

    public override void Init(HeroSelectionMenuController heroSelectionController, BattleUnitSO battleUnitObject)
    {
        _heroAttributes = (HeroUnitSO)battleUnitObject;
        base.Init(heroSelectionController, battleUnitObject);
        _heroActionOnClick = HeroSelection;
        _lockedPanel.SetActive(((HeroUnitSO)BattleUnitObject).IsUnitLocked());
    }

    private void HeroSelection()
    {
        if (_heroAttributes.IsUnitLocked())
            return;

        if (_heroAttributes.IsSelected)
        {
            ToggleSelectionUI();
            _heroSelectionController.DeselectHero();
            ToggleHeroSelection(false);

        }
        else if (_heroSelectionController.CanSelectHero())
        {
            _heroSelectionController.SelectHero();
            ToggleSelectionUI();
            ToggleHeroSelection(true);
        }
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
        _battleManager.DamageBoss(BattleUnitObject.GetAttackPower());
        StopCoroutine(_heroAttackAnimationCoroutine);
    }

    private void OnBattleWon()
    {
        if (CurrentHP > 0) IncreaseXP();    // Only alive heroes can get XP
    }

    private void IncreaseXP()
    {
        _heroAttributes.IncreaseExperiencePoint();

        if (_heroAttributes.GetExperiencePoint() % GlobalSettings.RequiredXPAmountToIncreaseLevel == 0)
            IncreaseLevel(); // The hero's level increases in every 5 experience points.
        else
            PlayAttributeIncreaseVFX("+1 XP");
    }

    private void IncreaseLevel()
    {
        _heroAttributes.IncreaseLevel();
        GetBonusForLevelingUp();
    }

    // The hero gets bonus for HP and damage whenever levels up
    private void GetBonusForLevelingUp()
    {

        int increasedAttackPowerAmount = (int)(_heroAttributes.GetAttackPower() * GlobalSettings.BonusPercentageForLevelingUp);
        _heroAttributes.IncreaseAttackPower(increasedAttackPowerAmount);
        int increasedHPAmount = (int)(_heroAttributes.GetHP() * GlobalSettings.BonusPercentageForLevelingUp);
        _heroAttributes.IncreaseHP(increasedHPAmount);

        string increaseText = "+1 XP\n+1 Level\n+" + increasedAttackPowerAmount + " AP\n+"+ increasedHPAmount + " HP";
        PlayAttributeIncreaseVFX(increaseText);
    }

    public override void Die()
    {
        base.Die();
        _battleManager.RemoveHeroFromAliveList(this);
    }

    public void ShowInfoPopUp()
    {
        _infoUI.Init(_heroAttributes);
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
        if (!_isHoldCompleted) _heroActionOnClick();
        StopHoldRoutine();
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

    private void StopHoldRoutine()
    {
        if (_holdRoutine != null) StopCoroutine(_holdRoutine);
        _isHoldCompleted = false;
        CloseInfoPopUp();
    }
    
    private void PlayAttributeIncreaseVFX(string attributeIncreaseText)
    {
        StartCoroutine(IncreaseAttributeVFX(attributeIncreaseText));
    }


    private IEnumerator IncreaseAttributeVFX(string text)
    {
        Vector2 initialPosition = _animatedText.transform.position;
        _animatedText.text = text;
        _animatedText.gameObject.SetActive(true);
        _animatedText.transform.DOLocalMove(Vector3.up * 10, 2f);
        yield return new WaitForSeconds(1f);
        _animatedText.DOFade(0, 2f).OnComplete(() =>
            {
                _animatedText.gameObject.SetActive(false);
                _animatedText.transform.position = initialPosition;

            }
        );;
    }

    public void ToggleHeroSelection(bool state) => _heroAttributes.IsSelected = state;
}
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiContainer;

    [SerializeField] private Text _nameText;
    [SerializeField] private Text _damageText;
    [SerializeField] private Text _hpText;
    [SerializeField] private Text _experienceText;
    [SerializeField] private Text _levelText;

    internal bool _isHoldCompleted;
    private float _timer;
    private Button _button;
    private Coroutine _holdRoutine;
    private float _tapAndHoldDuration = GlobalSettings.TapAndHoldDuration;

    public void Init(HeroUnitSO battleUnitObject)
    {
        _nameText.text = battleUnitObject.name;
        _damageText.text = battleUnitObject.GetAttackPower().ToString();
        _hpText.text = battleUnitObject.GetHP().ToString();
        _experienceText.text = battleUnitObject.GetExperiencePoint().ToString();
        _levelText.text = battleUnitObject.GetLevel().ToString();
    }
    public void OnPointerDown()
    {
        _holdRoutine = StartCoroutine(nameof(HoldTimerRoutine));
    }

    public void OnPointerUp()
    {
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

    private void ShowInfoPopUp() => _uiContainer.SetActive(true);
    private void CloseInfoPopUp() => _uiContainer.SetActive(false);
}
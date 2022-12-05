using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroSelectionMenuController : MonoBehaviour
{
    [SerializeField] private Button _battleButton;
    [SerializeField] private Transform _heroContainerTransform;
    [SerializeField] private GameObject _heroObject;
    [SerializeField] private BattleUnitInventorySO _heroInventory;
    [SerializeField] private GameObject _infoPopUp;

    private int _selectedHeroCount = 0;

    private UnitInfoUI _infoUI;

    private Vector3 _infoUIOffset = new Vector3(230f, 140f, 0);

    private void Awake()
    {
        _infoUI = _infoPopUp.GetComponent<UnitInfoUI>();
        _battleButton.onClick.AddListener(OpenBattleScene);
    }
    
    private void Start()
    {
        for (int i = 0; i < _heroInventory.BattleUnitObjects.Count; i++)
        {
            BattleUnit battleUnit = Instantiate(_heroObject, _heroContainerTransform).GetComponent<BattleUnit>();
            battleUnit.Init(this,_heroInventory.BattleUnitObjects[i]);
            battleUnit.BattleUnitObject.IsSelected = false;
        }
    }

    private void UpdateBattleButton() => _battleButton.interactable = _selectedHeroCount == 3;

    private void OpenBattleScene()
    {
        SceneManager.LoadScene(1);
    }

    public bool CanSelectHero() => _selectedHeroCount < 3;

    public void SelectHero(BattleUnit battleUnit)
    {
        _selectedHeroCount++;
        UpdateBattleButton();
    }
    
    public void DeselectHero(BattleUnit battleUnit)
    {
        _selectedHeroCount--;
        UpdateBattleButton();
    }
    
    public void CloseInfoPopUp()
    {
        _infoPopUp.SetActive(false);
    }

    public void ShowInfoPopUp(BattleUnit battleUnit)
    {
        _infoUI.Init(battleUnit.BattleUnitObject);
        _infoPopUp.transform.position= battleUnit.transform.position + _infoUIOffset;
        _infoPopUp.SetActive(true);
    }
}

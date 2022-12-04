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
        }
    }

    private void OpenBattleScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowInfoPopUp(BattleUnit battleUnit)
    {
        _infoUI.Init(battleUnit.BattleUnitObject);
        _infoPopUp.transform.position= battleUnit.transform.position + _infoUIOffset;
        _infoPopUp.SetActive(true);
    }
    
    public void CloseInfoPopUp()
    {
        _infoPopUp.SetActive(false);
    }
}

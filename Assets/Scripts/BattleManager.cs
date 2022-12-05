using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    private BattleState _currentState;

    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject heroPrefab;

    [SerializeField] private Transform bossTransform;
    [SerializeField] private Transform[] heroTransforms;

    private BattleUnit _bossUnit;

    [SerializeField] private Text _gameStateInfoText;
    [SerializeField] private GameOver _gameOverPanel;

    [SerializeField] private BattleUnitInventorySO _heroInventory;

    public delegate void OnBattleWon();

    public event OnBattleWon onBattleWon;

    private readonly List<BattleUnit> _aliveHeros = new List<BattleUnit>();

    public void OnEnable()
    {
        Init();
        _currentState = BattleState.Player_Turn;
    }

    private void Init()
    {
        CreateBoss();
        CreateHeroes();
    }

    private void CreateBoss()
    {
        BattleUnit battleUnit = Instantiate(bossPrefab, bossTransform).GetComponent<BattleUnit>();
        battleUnit.Init(this, battleUnit.BattleUnitObject);
        _bossUnit = battleUnit;
    }

    private void CreateHeroes()
    {
        int transformIndex = 0;
        for (int i = 0; i < _heroInventory.BattleUnitObjects.Count; i++)
        {
            if (_heroInventory.BattleUnitObjects[i].IsSelected)
            {
                BattleUnit battleUnit = Instantiate(heroPrefab, heroTransforms[transformIndex]).GetComponent<BattleUnit>();
                battleUnit.Init(this, _heroInventory.BattleUnitObjects[i]);
                _aliveHeros.Add(battleUnit);
                transformIndex++;
            }
        }
    }

    public void OnFightStarted()
    {
        ChangeState(BattleState.Fight_Animation);
    }

    private void ChangeState(BattleState newState)
    {
        _currentState = newState;
        Debug.Log("Current state: " + _currentState);

        switch (newState)
        {
            case BattleState.Player_Turn:
                _gameStateInfoText.text = "Your Turn!\n\n Select a hero to attack to the boss.";
                break;
            case BattleState.Fight_Animation:
                _gameStateInfoText.text = "Fight in action!";
                break;
            case BattleState.Boss_Turn:
                _gameStateInfoText.text = "Boss is preparing for an attack...";
                break;
            case BattleState.Win:
                _gameStateInfoText.text = "";
                _heroInventory.IncreaseFightCount();
                _gameOverPanel.SetPanel(true);
                break;
            case BattleState.Lose:
                _gameStateInfoText.text = "";
                _heroInventory.IncreaseFightCount();
                _gameOverPanel.SetPanel(false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public void DamageBoss(int damage)
    {
        _bossUnit.TakeDamage(damage);

        if (_bossUnit.CurrentHP <= 0)
            BattleWon();
        else
            BossTurn();
    }

    private void BattleWon()
    {
        ChangeState(BattleState.Win);
        onBattleWon.Invoke();
    }

    private void BossTurn()
    {
        ChangeState(BattleState.Boss_Turn);
        _bossUnit.Attack();
    }

    public bool IsPlayerTurn() => _currentState == BattleState.Player_Turn;

    public void DamageHero(BattleUnit heroUnit, int damage)
    {
        heroUnit.TakeDamage(damage);
        ChangeState(_aliveHeros.Count == 0 ? BattleState.Lose : BattleState.Player_Turn);
    }

    public void RemoveHeroFromAliveList(BattleUnit heroUnit) => _aliveHeros.Remove(heroUnit);
    public Transform GetBossLocation() => _bossUnit.transform;

    public BattleUnit GetRandomAliveHero()
    {
        int randomIndex = Random.Range(0, _aliveHeros.Count);
        return _aliveHeros[randomIndex];
    }
}

public enum BattleState
{
    Player_Turn,
    Fight_Animation,
    Boss_Turn,
    Win,
    Lose
}
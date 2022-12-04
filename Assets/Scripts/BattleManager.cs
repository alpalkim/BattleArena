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
    [SerializeField] private Transform hero1Transform;
    [SerializeField] private Transform hero2Transform;
    [SerializeField] private Transform hero3Transform;

    private BattleUnit _bossUnit;
    private BattleUnit _hero1Unit;
    private BattleUnit _hero2Unit;
    private BattleUnit _hero3Unit;

    [SerializeField] private Text _gameStateInfoText;
    [SerializeField] private GameOver _gameOverPanel;
    

    public delegate void OnBattleWon();

    public event OnBattleWon onBattleWon;

    private readonly List<BattleUnit> _aliveHeros = new List<BattleUnit>();

    public void OnEnable()
    {
        Init();
        _currentState = BattleState.Player_Turn;
        _aliveHeros.Add(_hero1Unit);
        _aliveHeros.Add(_hero2Unit);
        _aliveHeros.Add(_hero3Unit);
    }

    private void Init()
    {
        _bossUnit = CreateBattleUnit(bossPrefab, bossTransform);
        _hero1Unit = CreateBattleUnit(heroPrefab, hero1Transform);
        _hero2Unit = CreateBattleUnit(heroPrefab, hero2Transform);
        _hero3Unit = CreateBattleUnit(heroPrefab, hero3Transform);
    }

    private BattleUnit CreateBattleUnit(GameObject prefabToBeInstatiated, Transform transformToBeSet)
    {
        BattleUnit battleUnit = Instantiate(prefabToBeInstatiated, transformToBeSet).GetComponent<BattleUnit>();
        battleUnit.Init(this);
        return battleUnit;
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
                _gameOverPanel.SetPanel(true);
                break;
            case BattleState.Lose:
                _gameStateInfoText.text = "";
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

    public void DamageHero(BattleUnit heroUnit,int damage)
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